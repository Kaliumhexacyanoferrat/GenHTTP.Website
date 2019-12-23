using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using GenHTTP.Core;

using Project.Utilities;

namespace GenHTTP.Website
{

    public static class Program
    {

        public static async Task<int> Main(string[] args)
        {
            try
            {
                var project = Project.Create();

                var server = Server.Create()
                                   .Router(project)
                                   .Extension(new CacheExtension())
                                   .Console();

                using (var instance = server.Build())
                {
                    Console.WriteLine("Running ...");

#if DEBUG
                    Console.ReadLine();
#else
                    await Task.Run(() => Thread.Sleep(Timeout.Infinite));
#endif
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

    }

}
