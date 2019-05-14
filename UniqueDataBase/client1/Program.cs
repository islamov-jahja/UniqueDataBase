using System;
using System.Net.Http;
using DataBaseLib;

namespace client1
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase db = new DataBase("http://127.0.0.1:5001");
            Console.ReadKey();
            for(int i = 1; i < 2; i++)
            {
                Console.WriteLine(db.GetValue("aa"));
            }
            Console.ReadKey();
        }
    }
}
