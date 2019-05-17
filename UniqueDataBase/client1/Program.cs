using System;
using System.Net.Http;
using DataBaseLib;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace client1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase db = new DataBase("http://127.0.0.1:5001");
            DataBase db2 = new DataBase("http://127.0.0.1:5000");
            DataBase db3 = new DataBase("http://127.0.0.1:5002");
            DataBase db4 = new DataBase("http://127.0.0.1:5003");
            Console.ReadKey();
            String keyValue = null;
            while(keyValue != "end")
            {   
                keyValue = Console.ReadLine();
                db.SetValue(keyValue.Split(":")[0], keyValue.Split(":")[1]);
                Console.WriteLine(db.GetValue(keyValue.Split(":")[0]).Result);
                Console.WriteLine(db2.GetValue(keyValue.Split(":")[0]).Result);
                Console.WriteLine(db3.GetValue(keyValue.Split(":")[0]).Result);
                Console.WriteLine(db4.GetValue(keyValue.Split(":")[0]).Result);
            }
            Console.ReadKey();
        }
    }
}
