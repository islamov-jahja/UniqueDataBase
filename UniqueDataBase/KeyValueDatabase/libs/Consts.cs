using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace KeyValueDatabase.libs
{
   public class Consts
    {
        public const String PATH_TO_FILE_WITH_BASE = "../base.txt";
        public const String PATH_TO_FILE_WITH_URLS = "../config.txt";
        public const String HOST_PRESERVING_COMPONENT = "http://127.0.0.1:5010";
        public static String myURL;
    }
}