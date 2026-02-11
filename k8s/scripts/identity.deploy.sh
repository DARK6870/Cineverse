#!/bin/sh

cd ..
cd ..

docker build -t identity -f IdentityService/cicd/identity/docker/Dockerfile IdentityService --no-cache

kind load docker-image identity:latest --name cineverse

kubectl delete -f IdentityService/cicd/identity/deployment.yaml
kubectl apply -f IdentityService/cicd/identity/deployment.yaml