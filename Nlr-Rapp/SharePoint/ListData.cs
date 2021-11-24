using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;

using Nlr_Rapp.SharePoint.Settings;

namespace Nlr_Rapp.SharePoint
{
    public class ListData
    {
        private readonly ListDataSettings settings;
        private readonly GraphServiceClientProvider graphProvider;

        public ListData(IOptions<AppSettings> settings, GraphServiceClientProvider graphProvider)
        {
            this.settings = settings.Value.ListDataSettings;
            this.graphProvider = graphProvider;
        }

        [FunctionName("SharePoint-ListData")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log)
        {
            log.LogInformation("ListData is requested.");

            GraphServiceClient graph = graphProvider.Create();
            IListRequestBuilder list = await graph.GetListAsync(settings.SiteUrl, settings.ListName);

            List<QueryOption> queryOptions = new List<QueryOption>()
            {
                new QueryOption("select", "id"),
                new QueryOption("expand", "fields(select=Title,Author)")
            };
            IListItemsCollectionPage itemsPage = await list.Items
                .Request(queryOptions)
                .GetAsync();
            List<ListItem> items = new List<ListItem>(itemsPage);

            while (itemsPage.NextPageRequest != null)
            {
                itemsPage = await itemsPage.NextPageRequest.GetAsync();
                items.AddRange(itemsPage);
            }

            return new OkObjectResult(items);
        }
    }
}
