#!/bin/bash

docker build -t hugo:latest .

docker run --rm -v ./public/:/opt/HugoApp/public hugo
