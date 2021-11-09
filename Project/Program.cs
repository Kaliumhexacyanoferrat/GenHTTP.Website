using System.IO;
using System.IO.Compression;

using GenHTTP.Api.Protocol;

using GenHTTP.Engine;

using GenHTTP.Modules.ClientCaching;
using GenHTTP.Modules.Compression;
using GenHTTP.Modules.Practices;
using GenHTTP.Modules.ServerCaching;

namespace GenHTTP.Website
{

    public static class Program
    {
        private const string CACHE_DIR = "./cache";

        public static int Main(string[] args)
        {
            if (!Directory.Exists(CACHE_DIR))
            {
                Directory.CreateDirectory(CACHE_DIR);
            }

            var compression = CompressedContent.Default()
                                               .Level(CompressionLevel.Optimal);

            var cache = ServerCache.Persistent(CACHE_DIR)
                                   .Invalidate(false);

            var project = Project.Create()
                                 .Add(compression)
                                 .Add(cache);

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
