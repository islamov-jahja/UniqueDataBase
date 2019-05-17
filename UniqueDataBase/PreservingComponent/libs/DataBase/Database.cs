using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;

namespace PreservingComponent.libs
{
    public class DataBase
    {
        private static bool _wasSaved;
        private static bool _isWait; 
        private static volatile DataBase _instance;
        private static object _sync = new object();
        private Dictionary<String, String> _mapWithDataToSave = new Dictionary<String, String>();
        public string _adressToFile;
        private DataBase(string adressToFile)
        {
            _adressToFile = adressToFile;
            _isWait = false;
            _wasSaved = false;
        }

        public static DataBase GetInstance(string adressToFile = "../base.txt")
        {
            if (_instance == null)
            {
                lock (_sync)
                {
                    if (_instance == null)
                    {
                        _instance = new DataBase(adressToFile);
                    }
                }
            }

            return _instance;
        }

        public void UploadData(Dictionary<String, String> dict)
        {
            foreach (KeyValuePair<String, String> pair in dict)
            {
                if (_mapWithDataToSave.ContainsKey(pair.Key))
                {
                    String value;
                    _mapWithDataToSave.TryGetValue(pair.Key, out value);
                    if (value != pair.Value)
                    {
                        _wasSaved = false;
                        _mapWithDataToSave.Remove(pair.Key);
                        _mapWithDataToSave.Add(pair.Key, pair.Value);
                    }
                }else
                {
                    _wasSaved = false;
                    _mapWithDataToSave.Add(pair.Key, pair.Value);
                }
            }
        }

        public void SaveData()
        {
            if (!_wasSaved && !_isWait)
            {
                try{
                    StreamWriter file = new StreamWriter(_adressToFile);
                    foreach (KeyValuePair<String, String> pair in _mapWithDataToSave)
                        file.WriteLine($"{pair.Key}:{pair.Value}");

                    file.Close();
                    Console.WriteLine("saved");
                    _wasSaved = true;
                }catch
                {
                    if (!_isWait)
                    {
                        Task.Run(async () => {
                                    _isWait = true;
                                    Console.WriteLine("File not found. Next try in a minute");
                                    await Task.Delay(TimeSpan.FromMinutes(1));
                                    SaveData();
                                    _isWait = false;
                            });
                    }
                }
            }
        }

        ~DataBase()
        {
            SaveData();
        }
    }
}
