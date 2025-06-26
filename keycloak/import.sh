#!/bin/bash
set -e
if [ -z "$KEYCLOAK_URL" ]; then
  echo "KEYCLOAK_URL not set" >&2
  exit 1
fi
/opt/keycloak/bin/kc.sh import --dir=/config --override true
