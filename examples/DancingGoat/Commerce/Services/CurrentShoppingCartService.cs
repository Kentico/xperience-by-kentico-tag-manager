using System;
using System.Threading;
using System.Threading.Tasks;

using CMS.Commerce;
using CMS.ContactManagement;
using CMS.Core;
using CMS.DataEngine;
using CMS.Membership;

using Microsoft.AspNetCore.Http;

namespace DancingGoat.Commerce;

#pragma warning disable KXE0002 // Commerce feature is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

/// <summary>
/// Service for managing the current shopping cart.
/// </summary>
public class CurrentShoppingCartService
{
    private const string SHOPPING_CART_SESSION_KEY = "DancingGoat.ShoppingCart";
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly IInfoProvider<ShoppingCartInfo> shoppingCartInfoProvider;
    private readonly IInfoProvider<ContactInfo> contactInfoProvider;
    private readonly IUserInfoProvider userInfoProvider;


    public CurrentShoppingCartService(
        IHttpContextAccessor httpContextAccessor,
        IInfoProvider<ShoppingCartInfo> shoppingCartInfoProvider,
        IInfoProvider<ContactInfo> contactInfoProvider,
        IUserInfoProvider userInfoProvider)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.shoppingCartInfoProvider = shoppingCartInfoProvider;
        this.contactInfoProvider = contactInfoProvider;
        this.userInfoProvider = userInfoProvider;
    }


    /// <summary>
    /// Gets the current shopping cart.
    /// </summary>
    public Task<ShoppingCartInfo> Get(CancellationToken cancellationToken = default)
    {
        var session = httpContextAccessor.HttpContext?.Session;
        if (session == null)
        {
            return Task.FromResult<ShoppingCartInfo>(null);
        }

        var cartGuid = session.GetString(SHOPPING_CART_SESSION_KEY);
        if (string.IsNullOrEmpty(cartGuid) || !Guid.TryParse(cartGuid, out var guid))
        {
            return Task.FromResult<ShoppingCartInfo>(null);
        }

        var cart = shoppingCartInfoProvider.Get()
            .WhereEquals(nameof(ShoppingCartInfo.ShoppingCartGUID), guid)
            .TopN(1)
            .FirstOrDefault();

        return Task.FromResult(cart);
    }


    /// <summary>
    /// Creates a new shopping cart.
    /// </summary>
    public Task<ShoppingCartInfo> Create(string currencyCode, CancellationToken cancellationToken = default)
    {
        var cart = new ShoppingCartInfo
        {
            ShoppingCartGUID = Guid.NewGuid()
        };

        shoppingCartInfoProvider.Set(cart);

        var session = httpContextAccessor.HttpContext?.Session;
        session?.SetString(SHOPPING_CART_SESSION_KEY, cart.ShoppingCartGUID.ToString());

        return Task.FromResult(cart);
    }


    /// <summary>
    /// Discards the current shopping cart.
    /// </summary>
    public Task Discard(CancellationToken cancellationToken = default)
    {
        var session = httpContextAccessor.HttpContext?.Session;
        session?.Remove(SHOPPING_CART_SESSION_KEY);

        return Task.CompletedTask;
    }
}

#pragma warning restore KXE0002 // Commerce feature is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
