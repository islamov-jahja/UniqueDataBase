using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace DataBaseLib
{
    public class DataBase
    {
        private String _dataBaseHost;
        public DataBase(String dataBaseHost)
        {
            _dataBaseHost = dataBaseHost;
        }

        public async Task<string> GetValue(String key)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync($"{_dataBaseHost}/api/values/getValue", key);
            
            using(HttpContent responseContent = response.Content)
            {
                return await responseContent.ReadAsStringAsync();
            }
        }
    }
}
