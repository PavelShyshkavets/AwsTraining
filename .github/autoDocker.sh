#!/bin/bash

docker stop $(docker ps -a -q)
docker login -u AWS -p $(aws ecr get-login-password --region us-east-1) https://028922724832.dkr.ecr.us-east-1.amazonaws.com
docker pull 028922724832.dkr.ecr.us-east-1.amazonaws.com/book-api-repo
docker run -t -p 80:80 028922724832.dkr.ecr.us-east-1.amazonaws.com/book-api-repo
