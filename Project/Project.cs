using System;
using System.Collections.Generic;
using System.Text;

using GenHTTP.Api.Modules;

using GenHTTP.Modules.Core;
using GenHTTP.Modules.Core.Layouting;
using GenHTTP.Modules.Scriban;

namespace GenHTTP.Website
{

    public static class Project
    {

        public static IRouterBuilder Create()
        {
            var template = ModScriban.Template(Data.FromResource("Template.html"));

            var layout = Layout.Create()
                               .Template(template)
                               .Add("res", Static.Resources("Resources"))
                               .Add("documentation", GetDocumentation())
                               .AddPage("home", "Home")
                               .AddPage("legal", "Legal")
                               .Index("home");

            return layout;
        }

        private static IRouterBuilder GetDocumentation()
        {
            return Layout.Create()
                         .AddPage("intro", "Intro", "Getting started")
                         .Index("intro");
        }

        private static LayoutBuilder AddPage(this LayoutBuilder layout, string route, string file, string? title = null)
        {
            return layout.Add(route, ModScriban.Page(Data.FromResource($"{file}.html")).Title(title ?? file));
        }

    }

}
