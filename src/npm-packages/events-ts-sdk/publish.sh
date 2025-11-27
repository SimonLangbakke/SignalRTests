#!/usr/bin/env bash
set -e

VERSION_TYPE="patch"
ORVAL_MODE="Download"

while [[ "$#" -gt 0 ]]; do
  case "$1" in
    --version-type) VERSION_TYPE="$2"; shift 2;;
    --orval-mode)   ORVAL_MODE="$2"; shift 2;;
    *) echo "Unknown option: $1"; exit 1;;
  esac
done

if [[ ! "$VERSION_TYPE" =~ ^(patch|minor|major)$ ]]; then
  echo "ERROR: version type must be patch|minor|major"
  exit 1
fi

if [[ ! "$ORVAL_MODE" =~ ^(Http|Download)$ ]]; then
  echo "ERROR: orval-mode must be Http|Download"
  exit 1
fi

# Repo root
REPO_ROOT="$(cd "$(dirname "$0")" && pwd)"

SWAGGER_URL="http://localhost:5121/swagger/v1/swagger.json"
CONFIG_PATH="$REPO_ROOT/orval.config.js"
LOCAL_SPEC="$REPO_ROOT/../../../openapi.json"
BACKUP_PATH="$CONFIG_PATH.bak"

echo "Ensure API is running at $SWAGGER_URL"
read -p "Is it running? (y/n) " confirm
[[ "$confirm" =~ ^[yY]$ ]] || exit 1

# --- ORVAL ---
if [[ "$ORVAL_MODE" == "Http" ]]; then
    echo "Temporarily modifying Orval config"

    cp "$CONFIG_PATH" "$BACKUP_PATH"

    LOCAL_LINE="      target: '../../../openapi.json',"
    HTTP_LINE="      target: 'http://localhost:5121/swagger/v1/swagger.json',"

    if grep -q "$LOCAL_LINE" "$CONFIG_PATH"; then
        sed -i "s|$LOCAL_LINE|$HTTP_LINE|" "$CONFIG_PATH"
    else
        echo "WARNING: Expected line not found"
    fi

    orval --config "$CONFIG_PATH"

    echo "Restoring original Orval config"
    mv "$BACKUP_PATH" "$CONFIG_PATH"

else
    echo "Downloading spec → $LOCAL_SPEC"
    curl -sS "$SWAGGER_URL" -o "$LOCAL_SPEC"

    echo "Running Orval with unchanged config"
    orval --config "$CONFIG_PATH"
fi

# --- NPM + publish ---
if [[ -z "$GITHUB_NODE_AUTH_TOKEN" ]]; then
  echo "ERROR: GITHUB_NODE_AUTH_TOKEN not set"
  exit 1
fi

# TEMP switch to /src
pushd "$REPO_ROOT/src" >/dev/null

npm version "$VERSION_TYPE"
npm run build
npm publish

popd >/dev/null

echo "✅ Done!"
