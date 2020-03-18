using System.Collections.Generic;

using GenHTTP.Api.Routing;
using GenHTTP.Modules.Core;
using GenHTTP.Modules.Core.Layouting;
using GenHTTP.Modules.Scriban;
using GenHTTP.Themes.Lorahost;

namespace GenHTTP.Website
{

    public static class Project
    {

        public static IRouterBuilder Create()
        {
            var menu = Menu.Empty()
                           .Add("home", "Home")
                           .Add("features", "Features")
                           .Add("documentation/", "Documentation", new List<(string, string)> { ("content/", "Providing Content"), ("server/", "Server Setup"), ("hosting/", "Hosting Apps") })
                           .Add("links", "Links")
                           .Add("https://github.com/Kaliumhexacyanoferrat/GenHTTP", "Source")
                           .Add("legal", "Legal");

            var theme = Theme.Create()
                             .Header(Data.FromResource("Header.jpg"))
                             .Title("GenHTTP Webserver")
                             .Subtitle("Lightweight, embeddable web server written in pure C# with few dependencies to 3rd-party libraries. Compatible with .NET Standard 2.1.")
                             .Action("documentation/", "Get started");

            var website = Modules.Core.Website.Create()
                                              .Theme(theme)
                                              .Menu(menu)
                                              .AddScript("highlight.js", Data.FromResource("highlight.js"))
                                              .AddStyle("highlight.css", Data.FromResource("highlight.css"))
                                              .Content(GetLayout());
            return website;
        }

        #region Pages

        private static IRouterBuilder GetLayout()
        {
            return Layout.Create()
                         .Add("documentation", GetDocumentation())
                         .Add("images", Static.Resources("Images"))
                         .AddPage("home", "Home")
                         .AddPage("features", "Features")
                         .AddPage("legal", "Legal")
                         .AddPage("links", "Links", "Links & References")
                         .Index("home");
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

        private static IRouterBuilder GetContent()
        {
            return Layout.Create()
                         .AddPage("index", "Content.Index", "Providing Content")
                         .AddPage("templates", "Templates")
                         .AddPage("pages", "Pages")
                         .AddPage("themes", "Themes")
                         .AddPage("static-content", "StaticContent", "Static Content")
                         .AddPage("downloads", "Downloads")
                         .AddPage("redirects", "Redirects")
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

        #endregion

    }

}
