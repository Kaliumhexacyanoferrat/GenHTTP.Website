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
                         .Add("downloads", Static.Resources("Downloads"))
                         .AddPage(null, "Home", "C# Webserver Library", "Lightweight, embeddable web server written in pure C# with few dependencies to 3rd-party libraries.")
                         .AddMarkdownPage("features", "Features", null, "Features of the GenHTTP application framework such as performance, SEO or security.")
                         .AddMarkdownPage("legal", "Legal", null, "Legal information regarding GenHTTP.org")
                         .AddMarkdownPage("links", "Links", "Links & References", "Projects using the GenHTTP webserver engine.");
        }

        private static IHandlerBuilder GetDocumentation()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Intro", "Getting started", "Simple tutorial to setup a web application using the GenHTTP framework.")
                         .Add("content", GetContent())
                         .Add("server", GetServer())
                         .Add("hosting", GetHosting());
        }

        private static IHandlerBuilder GetContent()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Content.Index", "Providing Content", "Tutorials to write web applications (such as webservices or websites) using the GenHTTP server framework.")
                         .AddMarkdownPage("layouting", "Layouting", null, "Easily break down your web application into logical sections of content.")
                         .AddMarkdownPage("websites", "Websites", null, "Serve a themed web application with basic features such as templating, theming, sitemaps, or robots instruction files.")
                         .AddMarkdownPage("static-content", "StaticContent", "Static Content", "Provide resources stored on the file system or within an assembly via HTTP.")
                         .AddMarkdownPage("downloads", "Downloads", null, "Provide files via HTTP to requesting clients as a download, e.g. from the file system.")
                         .AddMarkdownPage("redirects", "Redirects", null, "Redirects requesting clients to another internal or external resource.")
                         .AddMarkdownPage("listing", "Listing", "Directory Browsing", "Simple web application to list the contents of a directory via HTTP.")
                         .AddMarkdownPage("reverse-proxies", "ReverseProxy", "Reverse Proxies", "Server component to relay requests to an upstream server and return the result to requesting clients.")
                         .AddMarkdownPage("virtual-hosts", "VirtualHosts", "Virtual Hosts", "Allows to handle requests depending on the host specified by the client (to serve multiple domains using a single server).")
                         .AddMarkdownPage("webservices", "Webservices", null, "Provide REST based web services in C# that can be consumed by clients to retrieve a JSON or XML serialized result.")
                         .AddMarkdownPage("authentication", "Authentication", null, "Restrict the content provided by the server to authenticated users.")
                         .AddMarkdownPage("single-page-applications", "SinglePageApplications", "Single Page Applications (SPA)", "Simple way to host applications written with JS frameworks such as Vue.js, Angular or React.")
                         .AddMarkdownPage("load-balancing", "LoadBalancer", "Load Balancer", "Simple way to distribute load on specified HTTP webservers or different file systems.")
                         .AddMarkdownPage("controllers", "Controllers", null, "Lightweight framework to write MVC web applications in C#.");
        }

        private static IHandlerBuilder GetServer()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Server.Index", "Server Setup", "Tutorials to configure the GenHTTP webserver for best practices regarding security or performance.")
                         .AddMarkdownPage("companions", "Companions", null, "Extend the GenHTTP server by a companion instance.")
                         .AddMarkdownPage("compression", "Compression", null, "Automatically compress responses generated by the webserver or add custom compression algorithms.")
                         .AddMarkdownPage("endpoints", "Endpoints", "Endpoints and Ports", "Configure the GenHTTP webserver to listen on different ports or endpoints.")
                         .AddMarkdownPage("security", "Security", "Secure Endpoints", "Configure the GenHTTP webserver for security.");
        }

        private static IHandlerBuilder GetHosting()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Hosting.Index", "Hosting Apps", "Host web applications written in C# using the .NET Core docker images.");
        }

        private static LayoutBuilder AddPage(this LayoutBuilder layout, string? route, string file, string? title , string description)
        {
            if (route != null)
            {
                return layout.Add(route, ModScriban.Page(Data.FromResource($"{file}.html")).Title(title ?? file).Description(description));
            }
            else
            {
                return layout.Index(ModScriban.Page(Data.FromResource($"{file}.html")).Title(title ?? file).Description(description));
            }
        }

        private static LayoutBuilder AddMarkdownPage(this LayoutBuilder layout, string? route, string file, string? title, string description)
        {
            if (route != null)
            {
                return layout.Add(route, ModMarkdown.Page(Data.FromResource($"{file}.md")).Title(title ?? file).Description(description));
            }
            else
            {
                return layout.Index(ModMarkdown.Page(Data.FromResource($"{file}.md")).Title(title ?? file).Description(description));
            }
        }

        #endregion

    }

}
