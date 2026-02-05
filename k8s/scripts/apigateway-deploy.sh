#!/bin/sh

cd ..
cd ..

docker build -t apigateway -f ApiGateway/cicd/apigateway/docker/Dockerfile ApiGateway --no-cache

kind load docker-image apigateway:latest --name cineverse

kubectl delete -f ApiGateway/cicd/apigateway/deployment.yaml
kubectl apply -f ApiGateway/cicd/apigateway/deployment.yaml