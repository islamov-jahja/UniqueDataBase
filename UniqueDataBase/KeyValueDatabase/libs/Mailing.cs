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
        public Mailing()
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

        public async Task<bool> MakeNewsletterAsync(KeyValuePair<String, String> pairKeyValue)
        {
            HttpClient client = new HttpClient();
            String message = $"{pairKeyValue.Key}:{pairKeyValue.Value}";

            foreach(string port in urls)
            {
                await client.PostAsJsonAsync($"http://127.0.0.1:{port}/api/values", message);
            }

            return true;
        }

        public void ShowUrls()
        {
            Console.WriteLine("AAA    " + urls.Count());
            foreach(String value in urls)
                Console.WriteLine($"AAAAAAAA    {value}");
        }
        
        private List<String> urls = new List<String>();
    }
}