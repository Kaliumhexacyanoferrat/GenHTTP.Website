using GenHTTP.Core;
using Project.Utilities;

namespace GenHTTP.Website
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            var project = Project.Create();

            return Host.Create()
                       .Router(project)
                       .Extension(new CacheExtension())
                       .Console()
                       .Run();
        }

    }

}
