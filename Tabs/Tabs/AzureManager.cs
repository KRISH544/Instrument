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
            this.Module2kmai806table = this.client.GetTable<module2kmai806table>();
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

        internal IMobileServiceTable<module2kmai806table> Module2kmai806table { get => Module2kmai806table1; set => Module2kmai806table1 = value; }
        internal IMobileServiceTable<module2kmai806table> Module2kmai806table1 { get => module2kmai806table; set => module2kmai806table = value; }
        internal IMobileServiceTable<module2kmai806table> Module2kmai806table2 { get => module2kmai806table; set => module2kmai806table = value; }

        public async Task<List<module2kmai806table>> getRows ()
        {
            return await this.Module2kmai806table.ToListAsync();
        }
    }
}
