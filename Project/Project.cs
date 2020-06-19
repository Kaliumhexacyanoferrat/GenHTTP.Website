using System.Collections.Generic;

using GenHTTP.Api.Content;
using GenHTTP.Modules.Core;
using GenHTTP.Modules.Core.Layouting;
using GenHTTP.Modules.Scriban;

using GenHTTP.Themes.Lorahost;

namespace GenHTTP.Website
{

    public static class Project
    {

        public static IHandlerBuilder Create()
        {
            var menu = Menu.Empty()
                           .Add("{website}", "Home")
                           .Add("features", "Features")
                           .Add("documentation/", "Documentation", new List<(string, string)> { ("content/", "Providing Content"), ("server/", "Server Setup"), ("hosting/", "Hosting Apps") })
                           .Add("links", "Links")
                           .Add("https://github.com/Kaliumhexacyanoferrat/GenHTTP", "Source")
                           .Add("legal", "Legal");

            var theme = Theme.Create()
                             .Header(Data.FromResource("Header.jpg"))
                             .Title("GenHTTP Webserver")
                             .Subtitle("Lightweight, embeddable web server library written in pure C# with few dependencies to 3rd-party libraries. Compatible with .NET Standard 2.1.")
                             .Action("documentation", "Get started");

            var website = Modules.Core.Website.Create()
                                              .Theme(theme)
                                              .Menu(menu)
                                              .AddScript("highlight.js", Data.FromResource("highlight.js"))
                                              .AddStyle("highlight.css", Data.FromResource("highlight.css"))
                                              .Favicon(Data.FromResource("favicon.ico"))
                                              .Content(GetLayout());
            return website;
        }

        #region Pages

        private static IHandlerBuilder GetLayout()
        {
            return Layout.Create()
                         .Add("documentation", GetDocumentation())
                         .Add("images", Static.Resources("Images"))
                         .AddPage(null, "Home", "C# Webserver Library")
                         .AddPage("features", "Features")
                         .AddPage("legal", "Legal")
                         .AddPage("links", "Links", "Links & References");
        }

        private static IHandlerBuilder GetDocumentation()
        {
            return Layout.Create()
                         .AddPage(null, "Intro", "Getting started")
                         .Add("content", GetContent())
                         .Add("server", GetServer())
                         .Add("hosting", GetHosting());
        }

        private static IHandlerBuilder GetContent()
        {
            return Layout.Create()
                         .AddPage(null, "Content.Index", "Providing Content")
                         .AddPage("websites", "Websites")
                         .AddPage("static-content", "StaticContent", "Static Content")
                         .AddPage("downloads", "Downloads")
                         .AddPage("redirects", "Redirects")
                         .AddPage("listing", "Listing", "Directory Browsing")
                         .AddPage("reverse-proxies", "ReverseProxy", "Reverse Proxies")
                         .AddPage("virtual-hosts", "VirtualHosts", "Virtual Hosts")
                         .AddPage("webservices", "Webservices")
                         .AddPage("authentication", "Authentication");
        }

        private static IHandlerBuilder GetServer()
        {
            return Layout.Create()
                         .AddPage(null, "Server.Index", "Server Setup")
                         .AddPage("companions", "Companions")
                         .AddPage("compression", "Compression")
                         .AddPage("endpoints", "Endpoints", "Endpoints and Ports")
                         .AddPage("security", "Security", "Secure Endpoints");
        }

        private static IHandlerBuilder GetHosting()
        {
            return Layout.Create()
                         .AddPage(null, "Hosting.Index", "Hosting Apps");
        }

        private static LayoutBuilder AddPage(this LayoutBuilder layout, string? route, string file, string? title = null)
        {
            if (route != null)
            {
                return layout.Add(route, ModScriban.Page(Data.FromResource($"{file}.html")).Title(title ?? file));
            }
            else
            {
                return layout.Index(ModScriban.Page(Data.FromResource($"{file}.html")).Title(title ?? file));
            }
        }

        #endregion

    }

}
