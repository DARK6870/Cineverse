#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  identity \
  IdentityService/cicd/identity/docker/Dockerfile \
  IdentityService \
  IdentityService/cicd/identity/deployment.yaml \
  cineverse
