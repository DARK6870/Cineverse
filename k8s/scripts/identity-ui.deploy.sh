#!/bin/sh

cd ..
cd ..

docker build -t identityui -f IdentityService/cicd/identityui/docker/Dockerfile IdentityService --no-cache

kind load docker-image identityui:latest --name cineverse

kubectl delete -f IdentityService/cicd/identityui/deployment.yaml
kubectl apply -f IdentityService/cicd/identityui/deployment.yaml