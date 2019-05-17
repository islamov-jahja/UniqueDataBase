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
        private Dictionary<String, String> _mapWithData = new Dictionary<String, String>();
        private DataBase()
        {
            ToSaveData();
            Mailing mailing = Mailing.GetInstance();
            mailing.BaseWasUpdated();

            StreamReader file = new StreamReader(Consts.PATH_TO_FILE_WITH_BASE);

            String[] keyValue;
            while(!file.EndOfStream)
            {
                keyValue = file.ReadLine().Split(':');
                _mapWithData.Add(keyValue[0], keyValue[1]);
            }

            file.Close();
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

        public bool setValue(String key, String value)
        {
            try{
                if(_mapWithData.ContainsKey(key))
                {
                    _mapWithData.Remove(key); 
                    _mapWithData.Add(key, value);
                }else
                {
                    _mapWithData.Add(key, value);
                }

                return true; 
            }
            catch
            {
                return false;
            }
        }

        public async void ToSaveData()
        {
            HttpClient client = new HttpClient();
            String json = JsonConvert.SerializeObject(_mapWithData);
            await client.PostAsJsonAsync($"{Consts.HOST_PRESERVING_COMPONENT}/save", json);
            Console.WriteLine("saved");
            await Task.Run(async () => {
                                await Task.Delay(TimeSpan.FromMinutes(1));
                                ToSaveData();
                        });
        }

        ~DataBase()
        {
            ToSaveData();
        }
    }
}