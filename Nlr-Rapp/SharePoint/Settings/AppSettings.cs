using System;
using System.Collections.Generic;
using System.Text;

namespace Nlr_Rapp.SharePoint.Settings
{
    public class AppSettings
    {
        public AzureAppSettings AzureAppSettings { get; set; }

        public ListDataSettings ListDataSettings { get; set; }
    }
}
