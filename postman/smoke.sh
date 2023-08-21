#!/bin/bash
set -e
ENV=$1
echo $ENV
ENV_FILE="CPRS - ${ENV}.postman_environment.json"
echo $ENV_FILE

npm install

echo "***** START *****"

npm run newman -- "StorageSpike.postman_collection.json" -e "${ENV_FILE}" -k -r cli,teamcity

echo "***** END *****"