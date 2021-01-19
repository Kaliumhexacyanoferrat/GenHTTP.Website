using GenHTTP.Api.Protocol;

using GenHTTP.Engine;

using GenHTTP.Modules.ClientCaching;
using GenHTTP.Modules.Practices;

namespace GenHTTP.Website
{

    public static class Program
    {

        public static int Main(string[] args)
        {
            var project = Project.Create();

            var cachePolicy = ClientCache.Policy()
                                         .Duration(7)
                                         .Predicate((_, r) => r.ContentType?.KnownType != ContentType.TextHtml);

            return Host.Create()
                       .Handler(project)
#if DEBUG
                       .Development()
#endif
                       .Add(cachePolicy)
                       .Defaults()
                       .Console()
                       .Run();
        }

    }

}
