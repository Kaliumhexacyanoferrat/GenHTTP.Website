using System.Collections.Generic;

using GenHTTP.Api.Content;
using GenHTTP.Api.Content.IO;
using GenHTTP.Api.Content.Templating;
using GenHTTP.Modules.Basics;
using GenHTTP.Modules.IO;
using GenHTTP.Modules.Layouting;
using GenHTTP.Modules.Layouting.Provider;
using GenHTTP.Modules.Markdown;
using GenHTTP.Modules.Pages;
using GenHTTP.Modules.Placeholders;
using GenHTTP.Modules.Razor;
using GenHTTP.Modules.Websites;
using GenHTTP.Modules.Websites.Sites;

using GenHTTP.Themes.Lorahost;

namespace GenHTTP.Website
{

    public static class Project
    {
        private static readonly IResource _ShareContent = Resource.FromAssembly("Share.html").Build();

        private static readonly IResource _VideoPlayer = Resource.FromAssembly("Video.cshtml").Build();

        public static WebsiteBuilder Create()
        {
            var menu = Menu.Empty()
                           .Add("{website}", "Home")
                           .Add("features", "Features")
                           .Add("documentation/", "Documentation", new List<(string, string)> { ("content/", "Providing Content"), ("testing/", "Testing Apps"), ("server/", "Server Setup"), ("hosting/", "Hosting Apps"), ("asp-net-comparison", "Comparison with ASP.NET") })
                           .Add("links", "Links")
                           .Add("legal", "Legal")
                           .Add("https://discord.gg/GwtDyUpkpV", "<span class=\"fab fa-discord\" style=\"font-size: 16pt; color: #fff; margin-right: 0;\" aria-label=\"GenHTTP on Discord\"></span>")
                           .Add("https://github.com/Kaliumhexacyanoferrat/GenHTTP", "<span class=\"fab fa-github\" style=\"font-size: 16pt; color: #fff; margin-right: 0;\" aria-label=\"GenHTTP on GitHub\"></span>");

            var theme = Theme.Create()
                             .Header(Resource.FromFile("./Resources/Header.jpg"))
                             .Title("GenHTTP Webserver")
                             .Subtitle("Simple and lightweight, embeddable HTTP server written in pure C# with few dependencies to 3rd-party libraries. Compatible with .NET 6/7/8.")
                             .Action("documentation/", "Get started");

            var website = Modules.Websites.Website.Create()
                                                  .Theme(theme)
                                                  .Menu(menu)
                                                  .AddScript("highlight.js", Resource.FromAssembly("highlight.js"))
                                                  .AddStyle("highlight.css", Resource.FromAssembly("highlight.css"))
                                                  .AddScript("shareon.iife.js", Resource.FromAssembly("shareon.iife.js"))
                                                  .AddStyle("shareon.min.css", Resource.FromAssembly("shareon.min.css"))
                                                  .AddScript("custom.js", Resource.FromAssembly("custom.js"))
                                                  .AddStyle("custom.css", Resource.FromAssembly("custom.css"))
                                                  .Favicon(Resource.FromFile("./Resources/favicon.ico"))
                                                  .Content(GetLayout());

            return website;
        }

        #region Pages

        private static IHandlerBuilder GetLayout()
        {
            return Layout.Create()
                         .Add("documentation", GetDocumentation())
                         .Add("cases", GetCases())
                         .AddPage(null, "Home", "C# HTTP Webserver Library", "Lightweight, embeddable HTTP web server written in pure C# with few dependencies to 3rd-party libraries.")
                         .AddMarkdownPage("features", "Features", null, "Features of the GenHTTP application framework such as performance, SEO or security.")
                         .AddMarkdownPage("legal", "Legal", null, "Legal information regarding GenHTTP.org", addSocial: false)
                         .AddMarkdownPage("links", "Links", "Links & References", "Projects using the GenHTTP webserver engine.")
                         .Add(Resources.From(ResourceTree.FromDirectory("./Resources/Public")));
        }

        private static IHandlerBuilder GetDocumentation()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Intro", "Getting started", "Simple tutorial to setup a web application using the GenHTTP framework.")
                         .Add("content", GetContent())
                         .Add("testing", GetTesting())
                         .Add("server", GetServer())
                         .Add("hosting", GetHosting())
                         .Add("asp-net-comparison", GetComparison());
        }

        private static IHandlerBuilder GetContent()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Content.Index", "Providing Content", "Tutorials to write web applications (such as webservices or websites) using the GenHTTP server framework.")
                         .AddMarkdownPage("layouting", "Layouting", null, "Easily break down your web application into logical sections of content.")
                         .AddMarkdownPage("websites", "Websites", "Websites", "Serve a themed web application with basic features such as templating, theming, sitemaps, or robots instruction files.")
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
                         .AddMarkdownPage("controllers", "Controllers", null, "Lightweight framework to write MVC web applications in C#.")
                         .AddMarkdownPage("cors", "CORS", null, "Automatically configure your webservices for Cross-Origin Resource Sharing.")
                         .AddMarkdownPage("concerns", "Concerns", "Custom Concerns", "Add behavior to all handlers within a section of your web application.")
                         .AddMarkdownPage("handlers", "Handlers", "Custom Handlers", "Low level handling of requests and generation of HTTP web server responses.")
                         .AddMarkdownPage("resources", "Resources", null, "An abstraction layer over file system capabilities to allow binary content to be fetched from any data source")
                         .AddMarkdownPage("compression", "Compression", null, "Automatically compress responses generated by the webserver or add custom compression algorithms.")
                         .AddMarkdownPage("static-websites", "StaticWebsites", "Static Websites", "Simple way to host static websites with sitemap and robots.txt support.")
                         .Add("client-caching", Redirect.To("/documentation/content/client-caching-validation"))
                         .AddMarkdownPage("client-caching-validation", "ClientCachingValidation", "Client Caching (Validation)", "Instructs clients to validate their cache when requesting resources to save bandwith and resources.")
                         .AddMarkdownPage("client-caching-policy", "ClientCachingPolicy", "Client Caching (Policy)", "Instructs clients to cache responses generated by the server for some time.")
                         .AddMarkdownPage("defaults", "Defaults", null, "Automatically configures your web server for performance and security.")
                         .AddMarkdownPage("server-caching", "ServerCaching", "Server Caching", "Caches responses generated by the server to server them faster when requested again.")
                         .AddMarkdownPage("ranges", "RangeSupport", "Range Support", "Enables partial responses if requested by the client, e.g. to resume downloads.")
                         .AddMarkdownPage("caches", "Caches", null, "Different backends allowing to store computation heavy work for improved performance.")
                         .AddMarkdownPage("templates", "Templates", null, "Create new webservices and websites in a couple of minutes.")
                         .AddMarkdownPage("combined-pages", "CombinedPages", "Combined Pages", "Assemble different kind of content into a single web page.")
                         .AddMarkdownPage("functional", "Functional", "Functional Handler", "Respond to HTTP requests in a functional manner with no boiler code.")
                         .AddMarkdownPage("error-handling", "ErrorHandling", "Error Handling", "Generates custom HTTP responses for exceptions and missing content.")
                         .AddMarkdownPage("conversion", "Conversion", "Serialization and Deserialization", "Configures the content serialization features of the webserver")
                         .AddMarkdownPage("injection", "Injection", "Parameter Injection", "Add custom parameter resolvers in your application framework")
                         .AddMarkdownPage("results", "Results", null, "Return structured data types from your API while still changing the response semantics");
        }

        private static IHandlerBuilder GetServer()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Server.Index", "Server Setup", "Tutorials to configure the GenHTTP webserver for best practices regarding security or performance.")
                         .AddMarkdownPage("companions", "Companions", null, "Extend the GenHTTP server by a companion instance.")
                         .Add("compression", Redirect.To("/documentation/content/compression"))
                         .AddMarkdownPage("endpoints", "Endpoints", "Endpoints and Ports", "Configure the GenHTTP webserver to listen on different ports or endpoints.")
                         .AddMarkdownPage("security", "Security", "Secure Endpoints", "Configure the GenHTTP webserver for security.");
        }

        private static IHandlerBuilder GetHosting()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Hosting.Index", "Hosting Apps", "Host web applications written in C# using the .NET docker images.", video: "hosting-in-docker");
        }

        private static IHandlerBuilder GetComparison()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Comparison.Index", "Comparing ASP.NET", "Overview of the alternatives to ASP.NET concepts provided by the GenHTTP framework.");
        }

        private static IHandlerBuilder GetTesting()
        {
            return Layout.Create()
                         .AddMarkdownPage(null, "Testing.Index", "Testing Apps", "Introduction to testing applications written by using the GenHTTP framework.");
        }

        private static IHandlerBuilder GetCases()
        {
            return Layout.Create()
                         .AddMarkdownPage("unity-game-webservice-api", "Unity", "Webservice APIs for Unity Games", "Write web service APIs for your Unity games in C#, fast and simple.");
        }

        private static LayoutBuilder AddPage(this LayoutBuilder layout, string? route, string file, string? title, string description)
        {
            if (route != null)
            {
                return layout.Add(route, Page.From(Resource.FromAssembly($"{file}.html")).Title(title ?? file).Description(description));
            }
            else
            {
                return layout.Index(Page.From(Resource.FromAssembly($"{file}.html")).Title(title ?? file).Description(description));
            }
        }

        private static LayoutBuilder AddMarkdownPage(this LayoutBuilder layout, string? route, string file, string? title, string description, string? video = null, bool addSocial = true)
        {
            var page = CombinedPage.Create()
                                   .AddMarkdown(Resource.FromAssembly($"{file}.md"))
                                   .Title(title ?? file)
                                   .Description(description);

            if (video != null)
            {
                page.AddRazor<ViewModel<string>>(_VideoPlayer, (r, h) => new(new ViewModel<string>(r, h, video)));
            }

            if (addSocial)
            {
                page.Add(_ShareContent.GetResourceAsStringAsync().Result);
            }

            if (route != null)
            {
                return layout.Add(route, page);
            }
            else
            {
                return layout.Index(page);
            }
        }

        #endregion

    }

}
