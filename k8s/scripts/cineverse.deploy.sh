#!/bin/sh

cd ..
cd ..

docker build -t cineverse -f Cineverse/cicd/cineverse/docker/Dockerfile Cineverse --no-cache

kind load docker-image cineverse:latest --name cineverse

kubectl delete -f Cineverse/cicd/cineverse/deployment.yaml
kubectl apply -f Cineverse/cicd/cineverse/deployment.yaml