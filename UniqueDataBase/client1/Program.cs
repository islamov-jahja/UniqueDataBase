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
            //Console.WriteLine("Press enter to start tests");
            Console.ReadKey();
            DataBase db = new DataBase("http://127.0.0.1:5001");
            DataBase db1 = new DataBase("http://127.0.0.1:5000");
            DataBase db2 = new DataBase("http://127.0.0.1:5002");
            //while(true)
            /* {
                string a = Console.ReadLine();
                db.SetValue(a.Split(':')[0], a.Split(':')[1]);
                db1.SetValue(a.Split(':')[0], a.Split(':')[1]);
            }*/
            Console.WriteLine("Client created object of Data bases: db, db1, db2");
            Console.WriteLine("Client set key <hello> with value <world> to Data base db");
            db.SetValue("hello", "world");
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.WriteLine("Client get value of key <hello> from data base: db:");
            Console.WriteLine(db.GetValue("hello").Result);
            Console.WriteLine("Client set key <hello> with value <other world> to Data base db2");
            db2.SetValue("hello", "other world");
            Console.WriteLine("Client get value of key <hello> from data base: db, db1, db2:");
            Console.WriteLine(db.GetValue("hello").Result);
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            Console.WriteLine(db1.GetValue("hello").Result);
            Thread.Sleep(TimeSpan.FromMilliseconds(500));
            Console.WriteLine(db2.GetValue("hello").Result);
            Thread.Sleep(TimeSpan.FromSeconds(3));
            Console.ReadKey();
            Console.WriteLine(db.GetValue("hello").Result);
            Console.WriteLine(db1.GetValue("hello").Result);
            Console.WriteLine(db2.GetValue("hello").Result);
            db2.SetValue("hello", "other world");
            Console.ReadKey();
            Console.ReadKey();
        }
    }
}
