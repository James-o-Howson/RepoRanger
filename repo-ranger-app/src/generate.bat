#!/bin/bash

# Check if swagger.json file exists
if [ -f "swagger.json" ]; then
  # Check if the /generated directory exists
  if [ ! -d "./src/app/generated" ]; then
    # If it doesn't exist, create it
    mkdir -p "./src/app/generated"
  fi

  # Clear out any existing files in the /generated directory
  rm -rf "./src/app/generated/*"

  # Navigate to the root directory
  cd ../../../

  # Generate TypeScript code using openapi-generator-cli
  openapi-generator-cli generate -i swagger.json -g typescript-angular --additional-properties=supportsES6=true,fileNaming=kebab-case,typescriptThreePlus=true -o src/app/generated
else
  echo "swagger.json file not found. Exiting script."
fi
