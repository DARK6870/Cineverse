#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  cineverse \
  Cineverse/cicd/cineverse/docker/Dockerfile \
  Cineverse \
  Cineverse/cicd/cineverse/deployment.yaml \
  cineverse
