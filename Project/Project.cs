using System;
using System.Collections.Generic;
using System.Text;

using GenHTTP.Api.Modules;

using GenHTTP.Modules.Core;
using GenHTTP.Modules.Scriban;

namespace GenHTTP.Website
{

    public static class Project
    {

        public static IRouterBuilder Create()
        {
            var template = ModScriban.Template(Data.FromResource("Template.html"));

            var index = ModScriban.Page(Data.FromResource("Index.html"))
                                  .Title("GenHTTP Webserver");

            var layout = Layout.Create()
                               .Template(template)
                               .Add("res", Static.Resources("Resources"))
                               .Add("index", index, true);

            return layout;
        }

    }

}
