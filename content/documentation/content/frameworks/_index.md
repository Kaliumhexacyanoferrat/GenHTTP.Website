---
title: Frameworks
weight: 2
cascade:
  type: docs
---

Frameworks are regular [handlers](../handlers/) that allow you to solve a problem (such as implementing
web services) in a typical flavor or style, shaping the overall appearance of your application code base.
For all frameworks listed below there are [template projects](../templates/) to quickly spin off a 
new application using this technology, already with best practices and sane configuration values applied.

## Service Frameworks

All the frameworks below allow you to implement RESTful web services. As they share a 
[common base](../concepts/definitions/), they offer the same features and can be used interchangeably. 
Therefore, you may choose the framework that fits your problem and personal coding style the best.
If needed, those handlers can also be mixed within a project (see [layouting](../handlers/layouting/)).

{{< cards >}}

  {{< card link="./webservices/" title="Webservices" >}}
  
  {{< card link="./functional/" title="Functional Handlers" >}}
  
  {{< card link="./controllers/" title="Controllers" >}}

{{< /cards >}}

## Other Frameworks

In addition to services, the frameworks below provide additional guidance for typical use cases:

{{< cards >}}

  {{< card link="./websockets/" title="Websockets" >}}

  {{< card link="./single-page-applications/" title="Single Page Applications (SPA)" >}}

  {{< card link="./static-websites/" title="Static Websites" >}}

{{< /cards >}}
