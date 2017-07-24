using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tabs.Model;

namespace Tabs
{
    class AzureManager
    {
        private static AzureManager instance;
        private MobileServiceClient client;
        private IMobileServiceTable<module2kmai806table> module2kmai806table;

        private AzureManager()
        {
            this.client = new MobileServiceClient("http://module2kmai806.azurewebsites.net");
            this.module2kmai806table = this.client.GetTable<module2kmai806table>();
        }

        public MobileServiceClient AzureClient
        {
            get
            {
                return client;
            }
        }

        public static AzureManager AzureManagerInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AzureManager();
                }
                return instance;
            }
        }

        public async Task<List<module2kmai806table>> getRows ()
        {
            return await this.module2kmai806table.ToListAsync();
        }
    }
}
