#!/bin/sh

cd ..
cd ..

docker build -t notification -f NotificationService/cicd/notificationservice/docker/Dockerfile NotificationService --no-cache

kind load docker-image notification:latest --name cineverse

kubectl delete -f NotificationService/cicd/notificationservice/deployment.yaml
kubectl apply -f NotificationService/cicd/notificationservice/deployment.yaml