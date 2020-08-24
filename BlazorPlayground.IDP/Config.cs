// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;

namespace BlazorPlayground.IDP
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
                    ClientId = "playgroundclient",
                    ClientSecrets = { new Secret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0".Sha256()) },
                    
                    AllowedGrantTypes = GrantTypes.Hybrid,

                    RequirePkce = false,

                    RedirectUris =
                    {
                        "https://localhost:44400/signin-oidc",
                    },
                    FrontChannelLogoutUri = "https://localhost:44400/signout-oidc",
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:44400/signout-callback-oidc",
                    },

                    AllowOfflineAccess = true,
                    AllowedScopes = { "openid", "profile", "email", "phone", "roles", "apiresource", "api" },

                    RequireConsent = false
                },
                
                // interactive public
                new Client
                {
                    ClientId = "blazorplayground.public",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    AllowedCorsOrigins = {"https://localhost:43350"},

                    // where to redirect to after login
                    RedirectUris = { "https://localhost:43350/authentication/login-callback" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "https://localhost:43350/authentication/logout-callback" },

                    Enabled = true,

                    AllowedScopes = { "openid", "profile", "email", "phone", "roles", "api" },
                }, 
            };
    }
}