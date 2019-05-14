using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace KeyValueDatabase.libs
{
   public class DataBase
    {
        private Dictionary<String, String> mapWithData = new Dictionary<String, String>();

        public String GetValue(String key)
        {
            String value = null;
            mapWithData.TryGetValue(key, out value);
            return value;
        }

        public bool setValue(String key, String value)
        {
            try{
                if(mapWithData.ContainsKey(key))
                {
                    mapWithData.Remove(key); 
                    mapWithData.Add(key, value);
                }else
                {
                    mapWithData.Add(key, value);
                }

                return true; 
            }
            catch
            {
                return false;
            }
        }
    }
}