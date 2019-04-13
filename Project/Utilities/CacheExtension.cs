using System;
using System.Collections.Generic;
using System.Text;

using GenHTTP.Api.Infrastructure;
using GenHTTP.Api.Protocol;

namespace Project.Utilities
{

    public class CacheExtension : IServerExtension
    {

        public void Intercept(IRequest request, IResponse response)
        {
            if (response.Status.KnownStatus == ResponseStatus.OK)
            {
                if (response.ContentType?.KnownType != ContentType.TextHtml)
                {
                    response.Expires = DateTime.Now.AddDays(1);
                }
            }
        }

    }

}
