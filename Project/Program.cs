using GenHTTP.Engine;

using GenHTTP.Modules.Practices;

namespace GenHTTP.Website
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            var project = Project.Create();

            return Host.Create()
                       .Handler(project)
#if DEBUG
                       .Development()
#endif
                       .Defaults()
                       .Console()
                       .Run();
        }

    }

}
