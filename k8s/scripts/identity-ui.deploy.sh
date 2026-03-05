#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  identityui \
  IdentityService/cicd/identityui/docker/Dockerfile \
  IdentityService \
  IdentityService/cicd/identityui/deployment.yaml \
  cineverse
