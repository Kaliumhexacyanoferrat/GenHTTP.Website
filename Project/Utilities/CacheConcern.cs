using System;
using System.Collections.Generic;

using GenHTTP.Api.Content;
using GenHTTP.Api.Protocol;

namespace Project.Utilities
{

    public class CacheConcern : IConcern
    {

        #region Get-/Setters

        public IHandler Content { get; }

        public IHandler Parent { get; }

        #endregion

        #region Initialization

        public CacheConcern(IHandler parent, Func<IHandler, IHandler> contentFactory)
        {
            Parent = parent;
            Content = contentFactory(this);
        }

        #endregion

        #region Functionality

        public IEnumerable<ContentElement> GetContent(IRequest request) => Content.GetContent(request);

        public IResponse? Handle(IRequest request)
        {
            var response = Content.Handle(request);

            if (response != null)
            {
                if (response.Status.KnownStatus == ResponseStatus.OK)
                {
                    if (response.ContentType?.KnownType != ContentType.TextHtml)
                    {
                        response.Expires = DateTime.Now.AddDays(30);
                    }
                }
            }

            return response;
        }

        #endregion

    }

}
