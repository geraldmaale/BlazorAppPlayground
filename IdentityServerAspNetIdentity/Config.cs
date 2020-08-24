// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResources.Phone(),
                new IdentityResource
                {
                    Name = "roles",
                    DisplayName = "Roles",
                    UserClaims = {JwtClaimTypes.Role}
                }
            };

        public static IEnumerable<ApiResource> ApiResources =>
            new ApiResource[]
            {
                new ApiResource(
                    "apiresource",
                    "API Resource",
                    new[] { JwtClaimTypes.Role })
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new ApiScope[]
            {
                new ApiScope("scope2"),
                new ApiScope("api"),
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                // interactive client using code flow + pkce
                new Client
                {
                    AccessTokenType = AccessTokenType.Jwt,
                    // AccessTokenLifetime = 300, // in seconds
                    UpdateAccessTokenClaimsOnRefresh = true,
                    ClientId = "interactive",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RequirePkce = false,

                    RedirectUris =
                    {
                        "https://localhost:44400/signin-oidc",
                        "https://localhost:44350/authentication/login-callback",
                    },
                    FrontChannelLogoutUri = "https://localhost:44400/signout-oidc",
                    // FrontChannelLogoutUri = "https://localhost:44300/authentication/logout-callback",
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44400/signout-callback-oidc",
                        "https://localhost:44350/authentication/logout-callback",
                        
                    },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "phone", "roles", "apiresource", "api" },

                    RequireConsent = false
                },
            };
    }
}