using System.Collections.Generic;

using GenHTTP.Api.Content;

using GenHTTP.Modules.IO;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Layouting.Provider;
using GenHTTP.Modules.Markdown;
using GenHTTP.Modules.Scriban;
using GenHTTP.Modules.Websites;

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
                                              .AddStyle("custom.css", Data.FromResource("custom.css"))
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
                         .AddMarkdownPage("features", "Features")
                         .AddMarkdownPage("legal", "Legal")
                         .AddMarkdownPage("links", "Links", "Links & References");
        }

        private static IHandlerBuilder GetDocumentation()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Intro", "Getting started")
                         .Add("content", GetContent())
                         .Add("server", GetServer())
                         .Add("hosting", GetHosting());
        }

        private static IHandlerBuilder GetContent()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Content.Index", "Providing Content")
                         .AddMarkdownPage("websites", "Websites")
                         .AddMarkdownPage("static-content", "StaticContent", "Static Content")
                         .AddMarkdownPage("downloads", "Downloads")
                         .AddMarkdownPage("redirects", "Redirects")
                         .AddMarkdownPage("listing", "Listing", "Directory Browsing")
                         .AddMarkdownPage("reverse-proxies", "ReverseProxy", "Reverse Proxies")
                         .AddMarkdownPage("virtual-hosts", "VirtualHosts", "Virtual Hosts")
                         .AddMarkdownPage("webservices", "Webservices")
                         .AddMarkdownPage("authentication", "Authentication")
                         .AddMarkdownPage("single-page-applications", "SinglePageApplications", "Single Page Applications (SPA)")
                         .AddMarkdownPage("load-balancing", "LoadBalancer", "Load Balancer");
        }

        private static IHandlerBuilder GetServer()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Server.Index", "Server Setup")
                         .AddMarkdownPage("companions", "Companions")
                         .AddMarkdownPage("compression", "Compression")
                         .AddMarkdownPage("endpoints", "Endpoints", "Endpoints and Ports")
                         .AddMarkdownPage("security", "Security", "Secure Endpoints");
        }

        private static IHandlerBuilder GetHosting()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Hosting.Index", "Hosting Apps");
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

        private static LayoutBuilder AddMarkdownPage(this LayoutBuilder layout, string? route, string file, string? title = null)
        {
            if (route != null)
            {
                return layout.Add(route, ModMarkdown.Page(Data.FromResource($"{file}.md")).Title(title ?? file));
            }
            else
            {
                return layout.Index(ModMarkdown.Page(Data.FromResource($"{file}.md")).Title(title ?? file));
            }
        }

        #endregion

    }

}
