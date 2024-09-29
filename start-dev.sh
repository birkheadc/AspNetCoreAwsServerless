#!/bin/bash
clear
echo "Starting API..."
ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_AWS_REGION=ap-southeast-2 ASPNETCORE_COGNITO_USER_POOL_ID=Dpsfkfrj8 DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1 dotnet watch --project src/AspNetCoreAwsServerless/AspNetCoreAwsServerless.csproj