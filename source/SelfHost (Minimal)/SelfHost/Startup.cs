﻿using Owin;
using SelfHost.Config;
using Thinktecture.IdentityServer.Core.Configuration;
using Thinktecture.IdentityServer.Host.Config;

namespace SelfHost
{
    internal class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var factory = InMemoryFactory.Create(
                users:   Users.Get(), 
                clients: Clients.Get(), 
                scopes:  Scopes.Get());

            var options = new IdentityServerOptions
            {
                IssuerUri = "https://idsrv3.com",
                SiteName = "Thinktecture IdentityServer v3 - beta 2 (SelfHost)",
                RequireSsl = false,

                SigningCertificate = Certificate.Get(),
                Factory = factory,
                AccessTokenValidationEndpoint = EndpointSettings.Enabled
            };

            appBuilder.UseIdentityServer(options);
        }
    }
}