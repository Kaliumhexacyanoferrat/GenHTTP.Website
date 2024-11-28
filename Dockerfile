FROM alpine:latest

RUN apk add --update hugo go git

WORKDIR /opt/HugoApp

COPY . .

ENTRYPOINT ["hugo"]
