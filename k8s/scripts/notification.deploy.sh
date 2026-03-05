#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
"$SCRIPT_DIR/deploy-common.sh" \
  notification \
  NotificationService/cicd/notificationservice/docker/Dockerfile \
  NotificationService \
  NotificationService/cicd/notificationservice/deployment.yaml \
  cineverse
