#!/bin/sh

cd ..
cd ..

docker build -t cineverseui -f Cineverse/cicd/cineverseui/docker/Dockerfile Cineverse --no-cache

kind load docker-image cineverseui:latest --name cineverse

kubectl delete -f Cineverse/cicd/cineverseui/deployment.yaml
kubectl apply -f Cineverse/cicd/cineverseui/deployment.yaml