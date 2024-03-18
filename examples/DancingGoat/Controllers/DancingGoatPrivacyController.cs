using CMS.ContactManagement;
using CMS.DataEngine;
using CMS.DataProtection;
using CMS.Helpers;
using DancingGoat;
using DancingGoat.Controllers;
using DancingGoat.Helpers.Generator;
using DancingGoat.Models;
using Kentico.Content.Web.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;

[assembly: RegisterWebPageRoute(PrivacyPage.CONTENT_TYPE_NAME, typeof(DancingGoatPrivacyController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Controllers
{
    public class DancingGoatPrivacyController : Controller
    {
        private const string SUCCESS_RESULT = "success";
        private const string ERROR_RESULT = "error";

        private readonly ICurrentCookieLevelProvider cookieLevelProvider;
        private readonly IConsentAgreementService consentAgreementService;
        private readonly IInfoProvider<ConsentInfo> consentInfoProvider;
        private readonly IPreferredLanguageRetriever currentLanguageRetriever;
        private ContactInfo currentContact;


        private ContactInfo CurrentContact
        {
            get
            {
                if (currentContact == null)
                {
                    currentContact = ContactManagementContext.CurrentContact;
                }

                return currentContact;
            }
        }


        public DancingGoatPrivacyController(
            ICurrentCookieLevelProvider cookieLevelProvider,
            IConsentAgreementService consentAgreementService,
            IInfoProvider<ConsentInfo> consentInfoProvider,
            IPreferredLanguageRetriever currentLanguageRetriever)
        {
            this.cookieLevelProvider = cookieLevelProvider;
            this.consentAgreementService = consentAgreementService;
            this.consentInfoProvider = consentInfoProvider;
            this.currentLanguageRetriever = currentLanguageRetriever;
        }


        public async Task<ActionResult> Index()
        {
            var model = new PrivacyViewModel();

            if (!IsDemoEnabled())
            {
                model.DemoDisabled = true;
            }

            model.Consents = await GetAgreedConsentsForCurrentContact();
            model.ShowSavedMessage = TempData[SUCCESS_RESULT] != null;
            model.ShowErrorMessage = TempData[ERROR_RESULT] != null;
            model.PrivacyPageUrl = HttpContext.Request.Path;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Agree")]
        public ActionResult Agree(string returnUrl, string consentName)
        {
            var consentToAgree = consentInfoProvider.Get(consentName);

            cookieLevelProvider.SetCurrentCookieLevel(Kentico.Web.Mvc.CookieLevel.All.Level);

            if (consentToAgree != null && CurrentContact != null)
            {
                consentAgreementService.Agree(CurrentContact, consentToAgree);

                TempData[SUCCESS_RESULT] = true;
            }
            else
            {
                TempData[ERROR_RESULT] = true;
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("/Revoke")]
        public ActionResult Revoke(string returnUrl, string consentName)
        {
            var consentToRevoke = consentInfoProvider.Get(consentName);

            if (consentToRevoke != null && CurrentContact != null)
            {
                consentAgreementService.Revoke(CurrentContact, consentToRevoke);

                TempData[SUCCESS_RESULT] = true;
            }
            else
            {
                TempData[ERROR_RESULT] = true;
            }

            return Redirect(returnUrl);
        }


        private async Task<IEnumerable<PrivacyConsentViewModel>> GetAgreedConsentsForCurrentContact()
        {
            return await consentInfoProvider.Get()
                .ToAsyncEnumerable()
                .SelectAwait(async consent => new PrivacyConsentViewModel
                {
                    Name = consent.ConsentName,
                    Title = consent.ConsentDisplayName,
                    Text = (await consent.GetConsentTextAsync(currentLanguageRetriever.Get())).ShortText,
                    Agreed = CurrentContact is not null && consentAgreementService.IsAgreed(CurrentContact, consent)
                }).ToListAsync();
        }


        private bool IsDemoEnabled()
        {
            return consentInfoProvider.Get(TrackingConsentGenerator.CONSENT_NAME) != null;
        }
    }
}
