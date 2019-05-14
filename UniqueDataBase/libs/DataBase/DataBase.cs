using System;

namespace DataBase
{
    public class DataBase
    {
        private String _dataBaseHost;
        public Class1(String dataBaseHost)
        {
            _dataBaseHost = dataBaseHost;
        }

        public async string GetValue(String key)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsJsonAsync(_dataBaseHost, key);
            
            using(HttpContent responseContent = response.Content)
            {
                return await responseContent.ReadAsStringAsync();
            }
        }
    }
}
