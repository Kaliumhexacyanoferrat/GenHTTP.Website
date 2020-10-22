using GenHTTP.Engine;

using GenHTTP.Modules.Practices;
using Project.Utilities;

namespace GenHTTP.Website
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            var project = Project.Create();

            return Host.Create()
                       .Handler(project)
                       .Add(new CacheContentConcernBuilder())
#if DEBUG
                       .Development()
#endif
                       .Defaults()
                       .Console()
                       .Run();
        }

    }

}
