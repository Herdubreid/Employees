using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Employees.Data
{
    public class GlobalAppState
    {
        private Celin.AIS.Server Server { get; set; }
        public List<F0601161Row> Employees { get; set; }
        public async Task<bool> Init()
        {
            try
            {
                await Server.AuthenticateAsync();
                var response = await Server.RequestAsync<F0601161Response>(new F0601161DatabrowserRequest());
                Employees = new List<F0601161Row>(response.fs_DATABROWSE_F0601161.data.gridData.rowset);
            }
            catch
            {
                return false;
            }
            return true;
        }
        public GlobalAppState(IConfiguration configuration)
        {
            Server = new Celin.AIS.Server(configuration["baseUrl"]);
            Server.AuthRequest.username = configuration["user"];
            Server.AuthRequest.password = configuration["password"];
            _ = Init();
        }
    }
}
