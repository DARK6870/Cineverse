#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  cineverseui \
  Cineverse/cicd/cineverseui/docker/Dockerfile \
  Cineverse \
  Cineverse/cicd/cineverseui/deployment.yaml \
  cineverse
