using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;

namespace KeyValueDatabase.libs
{
   public class Mailing
    {
        private static volatile Mailing _instance;
        private static object _sync = new object();

        public static Mailing GetInstance()
        {
            if (_instance == null)
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = new Mailing();
                    }
                }
            }

            return _instance;
        }
        private Mailing()
        {
            try
            {
                StreamReader fileWithUrls = new StreamReader(Consts.PATH_TO_FILE_WITH_URLS);
                String line;
                while(!fileWithUrls.EndOfStream)
                {
                    line = fileWithUrls.ReadLine();
                    urls.Add(line.Split(':')[1]);
                }

                urls.Remove(Consts.myURL);
            }catch
            {
                Console.WriteLine("file not found");
            }
        }

        public async Task<bool> MakeNewsletterAsync(String pairKeyValue)
        {
            String[] values = pairKeyValue.Split(':');
            HttpClient client = new HttpClient();
            String message = $"{values[0]}:{values[1]}";

            foreach(string port in urls)
            {
                await client.PostAsJsonAsync($"http://127.0.0.1:{port}/setWithoutSend", message);
            }

            return true;
        }
        

        public void BaseWasUpdated()
        {
            HttpClient client = new HttpClient();
            foreach(string port in urls)
            {
                try{
                    client.GetAsync($"http://127.0.0.1:{port}/api/values");
                    break;
                }catch
                {
                    continue;
                }
            }
        }

        private List<String> urls = new List<String>();
    }
}