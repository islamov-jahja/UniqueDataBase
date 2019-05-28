using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;

namespace KeyValueDatabase.libs
{
   public class Mailing
    {
        private static volatile Mailing _instance;
        private static object _sync = new object();
        private bool _taskChooseLeaderAdded;

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
            _taskChooseLeaderAdded = false;

            try
            {
                StreamReader fileWithUrls = new StreamReader(Consts.PATH_TO_FILE_WITH_URLS);
                String line;

                while(!fileWithUrls.EndOfStream)
                {
                    line = fileWithUrls.ReadLine();
                    urls.Add(line.Split(':')[1]);
                }

                urls.Remove(Consts.MY_URL);
            }catch
            {
                Console.WriteLine("file not found");
            }
        }

        public async Task<bool> MakeNewsletterAsync(String pairKeyValue)
        {
            DataBase db = DataBase.GetInstance();
            String[] values = pairKeyValue.Split(':');
            String message = $"{values[0]}:{values[1]}";
            HttpClient client = new HttpClient();

            if (db._portOfLeader == Consts.MY_URL)
            {
                MailingIfLeader(client, message);
            }else
            {
                if (db._portOfLeader == Consts.NO_LEADER)
                {
                    String port = await GetPortOfLeader();
                    if (port != null)
                    {
                        db._portOfLeader = port;
                        GetCopyOfOriginalDatabase();
                    }else
                    {
                        StartToVote();
                    }
                }else
                {
                    if (!await LeaderMessageWasSet(message))
                    {
                        StartToVote();
                    }
                }
            }


            return true;
        }

        public async void GetCopyOfOriginalDatabase()
        {
            DataBase db = DataBase.GetInstance();
            try{
                using(HttpClient client = new HttpClient()){
                        HttpResponseMessage response =  await client.PostAsJsonAsync($"http://127.0.0.1:{db._portOfLeader}/getDict", "");
                        Dictionary<String, String> dict =  JsonConvert.DeserializeObject<Dictionary<String, String>>(await response.Content.ReadAsStringAsync());
                        
                        foreach (KeyValuePair<string, string> pair in dict)
                        {
                            db.SetValue(pair.Key, pair.Value);
                        }
                    }
            }
            catch(Exception e)
            {
            }
        }
        private async void sendPostMessage(string port, string message, string adress)
        {
            try{
                using(HttpClient client = new HttpClient()){
                        await client.PostAsJsonAsync($"http://127.0.0.1:{port}/{adress}", message);
                    }
            }
            catch(Exception e)
            {
            }
        }

        private void StartToVote()
        {
            if (!_taskChooseLeaderAdded)
            {
                _taskChooseLeaderAdded = true;
                Console.WriteLine("StartToVote");
                DataBase db = DataBase.GetInstance();
                db._portOfLeader = Consts.NO_LEADER;
                db.ToVote(Consts.MY_URL);
                DataBase._condidateSAndVotes.Clear();
                
                foreach(string port in urls)    
                {
                    sendPostMessage(port, Consts.MY_URL, "toVote");
                }

                CreateTaskToSendMessageAboutLeader();
            }
        }


        private async void CreateTaskIfLoseTryToBeLeader()
        {
            Console.WriteLine("waiting");
            Random rm = new Random();
            await Task.Run(async () => {
                await Task.Delay(TimeSpan.FromSeconds(rm.Next(1, 10)));

                if (DataBase.GetInstance()._portOfLeader == Consts.NO_LEADER)
                {
                    _taskChooseLeaderAdded = false;
                    StartToVote();
                }
                else
                {
                    Console.WriteLine("leader in waiting:" + DataBase.GetInstance()._portOfLeader);
                    _taskChooseLeaderAdded = false;
                }       
            });
        }
        private async void CreateTaskToSendMessageAboutLeader()
        {
            DataBase db = DataBase.GetInstance();
            await Task.Run(async () => {
                await Task.Delay(TimeSpan.FromSeconds(5));
                DataBase._condidateSAndVotes.Add(Consts.MY_URL, 1);
                if (DataBase.GetInstance().IamLeader())
                {
                    foreach(string port in urls)    
                    {  
                        sendPostMessage(port, Consts.MY_URL, "setLeader"); 
                    }

                    db._portOfLeader = Consts.MY_URL;
                    _taskChooseLeaderAdded = false;
                }else
                {
                    CreateTaskIfLoseTryToBeLeader();
                }
            });
        }
        private async Task<bool> LeaderMessageWasSet(String message)
        {
            try{
                HttpClient client = new HttpClient();
                DataBase db = DataBase.GetInstance();

                HttpResponseMessage response = await client.PostAsJsonAsync($"http://127.0.0.1:{db._portOfLeader}/setValue", message);
                return JsonConvert.DeserializeObject<bool>(await response.Content.ReadAsStringAsync());
            }catch(Exception e)
            {
                return false;
            }
        }

        private void MailingIfLeader(HttpClient client, String message)
        {
            foreach(string port in urls)    
            {
                client.PostAsJsonAsync($"http://127.0.0.1:{port}/setWithoutSend", message);
            }
        }
        private async Task<string> GetPortOfLeader()
        {
            string portLeader = null;
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromMilliseconds(3000);

            foreach(string port in urls)
            {
                try{
                    portLeader = await client.GetStringAsync($"http://127.0.0.1:{port}/leader");
                }catch(Exception e)
                {
                    continue;
                }

                if (portLeader != Consts.NO_LEADER)
                {
                    break;
                }
            }

            if (portLeader != Consts.NO_LEADER && portLeader != null)
            {
                return portLeader;
            }
            else
            {
                return null;
            }
        }

        private List<String> urls = new List<String>();
    }
}