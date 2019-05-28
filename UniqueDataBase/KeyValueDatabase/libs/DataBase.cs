using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace KeyValueDatabase.libs
{
   public class DataBase
    {
        private static volatile DataBase _instance;
        private static object _sync = new object();
        private static Dictionary<String, String> _mapWithData = new Dictionary<String, String>();
        public static Dictionary<String, int> _condidateSAndVotes = new Dictionary<string, int>();
        private static Dictionary<String, String> _notMailingData = new Dictionary<string, string>();
        private string _tempPortOfLeader;
        public string _portOfLeader;  
        public bool isCondidate;

        private bool _isWait;

        public string GetDict()
        {
            return JsonConvert.SerializeObject(_mapWithData);
        }
        public void ToVote(string port)
        {
            int countOfVotes = 0;
            _condidateSAndVotes.TryGetValue(port, out countOfVotes);

            if(_condidateSAndVotes.ContainsKey(port))
            {
                _condidateSAndVotes.Remove(port);
                _condidateSAndVotes.Add(port, countOfVotes++);
            }else
            {
                _condidateSAndVotes.Add(port, 1);
            }
        }

        private DataBase()
        {
            _portOfLeader = Consts.NO_LEADER;
        }

        public async void UploadData(Dictionary<String, String> tempDict)
        {
            Mailing mailing = Mailing.GetInstance();
            foreach(KeyValuePair<String, String> pair in tempDict)
            {
                SetValue(pair.Key, pair.Value);
                await mailing.MakeNewsletterAsync($"{pair.Key}:{pair.Value}");
            }
        }

        public async void SendDataBase()
        {
            string json = JsonConvert.SerializeObject(_mapWithData);
            HttpClient client = new HttpClient();
            await client.PostAsJsonAsync($"http://127.0.0.1:{_portOfLeader}/sendDict", json);
        }
        public bool IamLeader()
        {
            Console.WriteLine("Count: " + _condidateSAndVotes.Count);
            if (_condidateSAndVotes.Count == 1)
            {
                foreach (KeyValuePair<String, int> pair in _condidateSAndVotes)
                {
                    if (pair.Key == Consts.MY_URL){
                        return true;
                    }
                }
            }
            
            return false;   
        }

        public static DataBase GetInstance()
        {
            if (_instance == null)
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = new DataBase();
                    }
                }
            }

            return _instance;
        }

        public String GetValue(String key)
        {
            String value = null;
            _mapWithData.TryGetValue(key, out value);
            return value;
        }

        public bool SetValue(String key, String value)
        {
            bool wasSaved = false;

            if (_portOfLeader == Consts.NO_LEADER)
            {
                wasSaved = SaveDataToDataBase(_notMailingData, key, value);
            }else
            {   
                wasSaved = SaveDataToDataBase(_mapWithData, key, value);
            }

            Console.WriteLine($"port leader: {_portOfLeader}");

            return wasSaved;
        }

        private bool SaveDataToDataBase(Dictionary<string, string> dictToSave,String key, String value)
        {
            try{
                if(dictToSave.ContainsKey(key))
                {
                    dictToSave.Remove(key); 
                    dictToSave.Add(key, value);
                }else
                {
                    dictToSave.Add(key, value);
                }
                
                _mapWithData = dictToSave;
                return true; 
            }
            catch
            {
                return false;
            }
        }

        ~DataBase()
        {
        }
    }
}