#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  apigateway \
  ApiGateway/cicd/apigateway/docker/Dockerfile \
  ApiGateway \
  ApiGateway/cicd/apigateway/deployment.yaml \
  cineverse
