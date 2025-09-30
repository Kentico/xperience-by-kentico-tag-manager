using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CMS.Commerce;
using CMS.ContentEngine;

using DancingGoat;
using DancingGoat.Commerce;
using DancingGoat.Helpers;
using DancingGoat.Models;
using DancingGoat.Services;

using Kentico.Commerce.Web.Mvc;
using Kentico.Content.Web.Mvc.Routing;

using Microsoft.AspNetCore.Mvc;

#pragma warning disable KXE0002 // Commerce feature is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
[assembly: RegisterWebPageRoute(ShoppingCart.CONTENT_TYPE_NAME, typeof(DancingGoatShoppingCartController), WebsiteChannelNames = new[] { DancingGoatConstants.WEBSITE_CHANNEL_NAME })]

namespace DancingGoat.Commerce;

/// <summary>
/// Controller for managing the shopping cart.
/// </summary>
public sealed class DancingGoatShoppingCartController : Controller
{
    // Note: Shopping cart service functionality has been removed in Xperience 30.10.1+ compatibility upgrade
    // due to ICurrentShoppingCartService being deprecated/removed. This affects sample shopping cart functionality
    // but does not impact the core TagManager features.
    private readonly ProductVariantsExtractor productVariantsExtractor;
    private readonly WebPageUrlProvider webPageUrlProvider;
    private readonly ProductRepository productRepository;

    public DancingGoatShoppingCartController(
        // Shopping cart service parameter removed in Xperience 30.10.1+ upgrade
        ProductVariantsExtractor productVariantsExtractor,
        WebPageUrlProvider webPageUrlProvider,
        ProductRepository productRepository)
    {
        // Shopping cart service assignment removed in Xperience 30.10.1+ upgrade
        this.productVariantsExtractor = productVariantsExtractor;
        this.webPageUrlProvider = webPageUrlProvider;
        this.productRepository = productRepository;
    }


    public Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        // Shopping cart functionality removed in Xperience 30.10.1+ upgrade due to deprecated APIs
        return Task.FromResult<IActionResult>(View(new ShoppingCartViewModel(new List<ShoppingCartItemViewModel>(), 0)));
    }


    [HttpPost]
    [Route("/ShoppingCart/HandleAddRemove")]
    public async Task<IActionResult> HandleAddRemove(int contentItemId, int quantity, int? variantId, string action, string languageName)
    {
        // Shopping cart operations removed in Xperience 30.10.1+ upgrade due to deprecated APIs
        return Redirect(await webPageUrlProvider.ShoppingCartPageUrl(languageName));
    }


    [HttpPost]
    [Route("/ShoppingCart/Add")]
    public async Task<IActionResult> Add(int contentItemId, int quantity, int? variantId, string languageName)
    {
        // Shopping cart operations removed in Xperience 30.10.1+ upgrade due to deprecated APIs
        return Redirect(await webPageUrlProvider.ShoppingCartPageUrl(languageName));
    }


    private static string FormatProductName(string productName, IDictionary<int, string> variants, int? variantId)
    {
        return variants != null && variantId != null && variants.TryGetValue(variantId.Value, out string variantValue)
            ? $"{productName} - {variantValue}"
            : productName;
    }


    /// <summary>
    /// Gets the current shopping cart or creates a new one if it does not exist.
    /// Note: Functionality removed in Xperience 30.10.1+ due to deprecated APIs.
    /// </summary>
    private Task<ShoppingCartInfo?> GetCurrentShoppingCart()
    {
        return Task.FromResult<ShoppingCartInfo?>(null);
    }
}
#pragma warning restore KXE0002 // Commerce feature is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
