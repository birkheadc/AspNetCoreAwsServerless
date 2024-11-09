#!/bin/bash
clear
echo "Starting API..."
ASPNETCORE_ENVIRONMENT=Development \
ASPNETCORE_COGNITO_CLIENT_ID="27efufoivmsq7uuhtdrs1rp6af" \
ASPNETCORE_COGNITO_URL="https://aspnetcoreserverless-development.auth.ap-southeast-2.amazoncognito.com" \
DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1 \
dotnet watch --project src/AspNetCoreAwsServerless/AspNetCoreAwsServerless.csproj