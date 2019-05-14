using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace KeyValueDatabase.libs
{
   public class Mailing
    {
        public Mailing()
        {
            StreamReader fileWithUrls = new StreamReader(Consts.PATH_TO_FILE_WITH_URLS);
            String line;
            while(!fileWithUrls.EndOfStream)
            {
                line = fileWithUrls.ReadLine();
                urls.Add(line.Split(':')[1]);
            }

            urls.Remove(Consts.myURL);
        }

        public bool MakeNewsletter(KeyValuePair<String, String> pairKeyValue)
        {

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