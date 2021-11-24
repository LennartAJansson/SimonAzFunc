using Azure.Identity;

using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Client;

using Nlr_Rapp.SharePoint.Settings;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Nlr_Rapp.SharePoint
{
    public class GraphServiceClientProvider
    {
        private readonly AzureAppSettings appSettings;

        public GraphServiceClientProvider(IOptions<AppSettings> settings)
        {
            appSettings = settings.Value.AzureAppSettings;
        }

        public GraphServiceClient Create()
        {
            var scopes = new[] { "User.Read" }; 
            var authorizationCode = "AUTH_CODE_FROM_REDIRECT";
            var options = new TokenCredentialOptions
            {
                AuthorityHost = AzureAuthorityHosts.AzurePublicCloud
            };
            var authCodeCredential = new AuthorizationCodeCredential(appSettings.Tenant, 
                appSettings.ClientId, appSettings.ClientSecret, authorizationCode, options);

            return new GraphServiceClient(authCodeCredential, scopes);
        }
    }
}
