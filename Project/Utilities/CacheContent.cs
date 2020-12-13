using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using GenHTTP.Api.Content;
using GenHTTP.Api.Protocol;

namespace Project.Utilities
{

    public class CacheContentConcern : IConcern
    {

        public IHandler Content { get; }

        public IHandler Parent { get; }

        public CacheContentConcern(IHandler parent, Func<IHandler, IHandler> contentFactory)
        {
            Parent = parent;
            Content = contentFactory(this);
        }

        public IEnumerable<ContentElement> GetContent(IRequest request) => Content.GetContent(request);

        public async ValueTask<IResponse?> HandleAsync(IRequest request)
        {
            var response = await Content.HandleAsync(request);

            if (response != null)
            {
                if (response.Status.KnownStatus == ResponseStatus.OK)
                {
                    if (response.ContentType?.KnownType != ContentType.TextHtml)
                    {
                        response.Expires = DateTime.Now.AddDays(7);
                    }
                }
            }

            return response;
        }

        public ValueTask PrepareAsync() => Content.PrepareAsync();

    }

    public class CacheContentConcernBuilder : IConcernBuilder
    {

        public IConcern Build(IHandler parent, Func<IHandler, IHandler> contentFactory)
        {
            return new CacheContentConcern(parent, contentFactory);
        }

    }

}
