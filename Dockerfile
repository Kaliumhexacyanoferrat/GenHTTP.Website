# Stage 1
FROM alpine:latest AS build

# Pin Hugo to match the CI build.
ARG HUGO_VERSION=0.165.0

# Install Go and Git (for module resolution) plus the runtime libraries the
# extended Hugo binary needs on musl, then download the pinned Hugo release.
RUN apk add --update --no-cache go git ca-certificates libstdc++ gcompat wget \
    && wget -qO /tmp/hugo.tar.gz "https://github.com/gohugoio/hugo/releases/download/v${HUGO_VERSION}/hugo_extended_${HUGO_VERSION}_linux-amd64.tar.gz" \
    && tar -xzf /tmp/hugo.tar.gz -C /usr/local/bin hugo \
    && rm /tmp/hugo.tar.gz

WORKDIR /opt/HugoApp

# Copy Hugo config into the container Workdir.
COPY . .

# Run Hugo in the Workdir to generate HTML.
RUN hugo --minify

# Stage 2
FROM alpine:latest

# nginx plus the brotli and zstd compression modules. All three come from the
# same alpine repo so the dynamic modules are ABI-compatible with the server.
RUN apk add --no-cache nginx nginx-mod-http-brotli nginx-mod-http-zstd \
    && mkdir -p /run/nginx

# Custom config: compression (gzip + brotli + zstd), cache headers and 404 page.
COPY nginx/nginx.conf /etc/nginx/nginx.conf
COPY nginx/default.conf /etc/nginx/conf.d/default.conf

# Set workdir to the NGINX default dir.
WORKDIR /usr/share/nginx/html

# Copy HTML from previous build into the Workdir.
COPY --from=build /opt/HugoApp/public .

# Expose port 80
EXPOSE 80/tcp

STOPSIGNAL SIGQUIT
CMD ["nginx", "-g", "daemon off;"]
