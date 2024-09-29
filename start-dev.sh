#!/bin/bash
clear
echo "Starting API..."
ASPNETCORE_ENVIRONMENT=Development  ASPNETCORE_COGNITO_USER_POOL_ID="ap-southeast-2_p9VUYZmuz" DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1 dotnet watch --project src/AspNetCoreAwsServerless/AspNetCoreAwsServerless.csproj