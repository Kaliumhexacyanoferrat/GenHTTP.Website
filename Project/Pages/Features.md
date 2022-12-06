## Application Performance

All components of the GenHTTP webserver are optimized to use best practices such as
resource bundling, caching, or compression to provide optimal performance. As these concerns
are already handled by the framework, developers are allowed to focus on their actual work.

![genhttp.org analyzed with pagespeed insights](/images/pagespeed.png)

*see [Google PageSpeed Insights](https://developers.google.com/speed/pagespeed/insights/?url=https%3A%2F%2Fgenhttp.org%2F)*

## Server Performance

In terms of raw HTTP protocol performance, the GenHTTP webserver is located in the middle segment compared to
other server implementations, serving more than 600k requests per second. Improving the performance of the server is
an ongoing task, especially since there is currently a lot of potential to be tapped.

![GenHTTP framework analyzed with TechEmpower FrameworkBenchmarks](/images/tfb.png)

*see [TechEmpower Web Framework Benchmarks](https://www.techempower.com/benchmarks/#section=data-r20&hw=ph&test=composite&a=2)*

## Footprint

Applications developed with the GenHTTP SDK are optimized for low disk space and memory requirements. A basic application
(such as this website) will consume about 30 MB of memory and 50 MB of disk space when
[running in Docker](/documentation/hosting/).

![The website of the GenHTTP websever running in docker](/images/footprint.png)

## Security

The GenHTTP webserver uses the default security mechanisms that ship with .NET. This results in
a good security grade of A+ as reported by SSL Labs.

![Security analysis of the GenHTTP website](/images/ssl_labs.png)

*see [SSL Labs Server Test](https://www.ssllabs.com/ssltest/analyze.html?d=genhttp.org&latest)*