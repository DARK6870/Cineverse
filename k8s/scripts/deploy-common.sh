#!/usr/bin/env sh

set -eu

SCRIPT_DIR="$(CDPATH= cd -- "$(dirname -- "$0")" && pwd)"
REPO_ROOT="$(CDPATH= cd -- "$SCRIPT_DIR/../.." && pwd)"

if [ "$#" -lt 4 ] || [ "$#" -gt 5 ]; then
  echo "Usage: $0 <image> <dockerfile> <context> <manifest> [namespace]" >&2
  exit 1
fi

IMAGE="$1"
DOCKERFILE="$2"
CONTEXT="$3"
MANIFEST="$4"
NAMESPACE="${5:-cineverse}"

KIND_CLUSTER_NAME="${KIND_CLUSTER_NAME:-cineverse}"
NO_CACHE="${NO_CACHE:-0}"
SKIP_BUILD="${SKIP_BUILD:-0}"
SKIP_LOAD="${SKIP_LOAD:-0}"
ROLLOUT_TIMEOUT="${ROLLOUT_TIMEOUT:-120s}"

if [ "$NO_CACHE" = "1" ]; then
  NO_CACHE_FLAG="--no-cache"
else
  NO_CACHE_FLAG=""
fi

cd "$REPO_ROOT"

if [ "$SKIP_BUILD" != "1" ]; then
  docker build -t "$IMAGE" -f "$DOCKERFILE" "$CONTEXT" $NO_CACHE_FLAG
fi

if [ "$SKIP_LOAD" != "1" ]; then
  kind load docker-image "$IMAGE:latest" --name "$KIND_CLUSTER_NAME"
fi

DEPLOYMENTS="$(awk '
  $1 == "kind:" && $2 == "Deployment" { in_deployment = 1; next }
  in_deployment && $1 == "name:" { print $2; in_deployment = 0 }
' "$MANIFEST")"

EXISTING_DEPLOYMENTS_FILE="$(mktemp)"
trap 'rm -f "$EXISTING_DEPLOYMENTS_FILE"' EXIT INT HUP

for deployment in $DEPLOYMENTS; do
  if kubectl get deployment "$deployment" -n "$NAMESPACE" >/dev/null 2>&1; then
    echo "$deployment" >> "$EXISTING_DEPLOYMENTS_FILE"
  fi
done

kubectl apply -f "$MANIFEST"

for deployment in $DEPLOYMENTS; do
  if grep -qx "$deployment" "$EXISTING_DEPLOYMENTS_FILE"; then
    kubectl rollout restart "deployment/$deployment" -n "$NAMESPACE"
  fi

  kubectl rollout status "deployment/$deployment" -n "$NAMESPACE" --timeout="$ROLLOUT_TIMEOUT"
done
