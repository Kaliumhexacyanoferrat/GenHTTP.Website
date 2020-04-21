using System;

using GenHTTP.Api.Content;

namespace Project.Utilities
{

    public class CacheConcernBuilder : IConcernBuilder
    {

        public IConcern Build(IHandler parent, Func<IHandler, IHandler> contentFactory)
        {
            return new CacheConcern(parent, contentFactory);
        }

    }

}
