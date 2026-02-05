#!/bin/sh

cd ..
cd ./configs

kubectl delete -f cineverse-config.yaml
kubectl apply -f cineverse-config.yaml