using System;
using System.Collections.Generic;
using System.Text;

using GenHTTP.Api.Routing;

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
                               .Add("projects", GetProjects())
                               .AddPage("home", "Home")
                               .AddPage("legal", "Legal")
                               .Index("home");

            return layout;
        }

        private static IRouterBuilder GetDocumentation()
        {
            return Layout.Create()
                         .AddPage("intro", "Intro", "Getting started")
                         .Add("content", GetContent())
                         .Add("server", GetServer())
                         .Add("hosting", GetHosting())
                         .Index("intro");
        }
        
        private static IRouterBuilder GetProjects()
        {
            return Layout.Create()
                         .AddPage("gateway", "Gateway", "GenHTTP Gateway");
        }

        private static IRouterBuilder GetContent()
        {
            return Layout.Create()
                         .AddPage("index", "Content.Index", "Providing Content")
                         .AddPage("templates", "Templates")
                         .AddPage("pages", "Pages")
                         .AddPage("static-content", "StaticContent", "Static Content")
                         .AddPage("downloads", "Downloads")
                         .AddPage("listing", "Listing", "Directory Browsing")
                         .AddPage("reverse-proxies", "ReverseProxy", "Reverse Proxies")
                         .AddPage("virtual-hosts", "VirtualHosts", "Virtual Hosts")
                         .AddPage("webservices", "Webservices")
                         .Index("index");
        }

        private static IRouterBuilder GetServer()
        {
            return Layout.Create()
                         .AddPage("index", "Server.Index", "Server Setup")
                         .AddPage("companions", "Companions")
                         .AddPage("compression", "Compression")
                         .AddPage("extensions", "Extensions")
                         .AddPage("endpoints", "Endpoints", "Endpoints and Ports")
                         .AddPage("security", "Security", "Secure Endpoints")
                         .Index("index");
        }

        private static IRouterBuilder GetHosting()
        {
            return Layout.Create()
                         .AddPage("index", "Hosting.Index", "Hosting Apps")
                         .Index("index");
        }

        private static LayoutBuilder AddPage(this LayoutBuilder layout, string route, string file, string? title = null)
        {
            return layout.Add(route, ModScriban.Page(Data.FromResource($"{file}.html")).Title(title ?? file));
        }

    }

}
