#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  adminui \
  Admin/cicd/adminui/docker/Dockerfile \
  Admin \
  Admin/cicd/adminui/deployment.yaml \
  cineverse
