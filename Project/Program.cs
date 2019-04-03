using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

using GenHTTP.Core;

using GenHTTP.Website.Utilities;

namespace GenHTTP.Website
{

    public static class Program
    {

        public static void Main(string[] args)
        {
            var project = Project.Create();

            var server = Server.Create()
                               .Router(project)
                               .Compression(new BrotliCompression());

            using (var instance = server.Build())
            {
                Console.WriteLine("Press any key to stop ...");
                Console.ReadLine();
            }
        }
        
    }

}
