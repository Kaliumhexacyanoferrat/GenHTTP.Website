# GenHTTP Website

Public website of the [GenHTTP webserver](https://github.com/Kaliumhexacyanoferrat/GenHTTP), served at [genhttp.org](https://genhttp.org/).

Built with [Hugo](https://gohugo.io/) and the [Hextra](https://github.com/imfing/hextra) theme.

## Prerequisites

- [Hugo](https://gohugo.io/installation/) (extended edition), matching the version used in CI (see `.github/workflows`)
- [Dart Sass](https://sass-lang.com/dart-sass/), required for CSS changes to render locally

## Development

```bash
hugo server
```

Serves the site locally with live reload.

## Build

```bash
hugo --minify
```

Builds the site into `public/`, matching the CI build.

## Docker

```bash
docker build -t genhttp-website .
```

Builds the site with Hugo and serves the result via Nginx.

## Content

Documentation pages live under `content/documentation/`, mirroring the structure of the GenHTTP framework. New pages can be created from the archetype in `archetypes/default.md`.
