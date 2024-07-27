#!/bin/bash
echo "Starting API..."
ASPNETCORE_ENVIRONMENT=Development DOTNET_WATCH_RESTART_ON_RUDE_EDIT=1 dotnet watch --project src/AspNetCoreAwsServerless/AspNetCoreAwsServerless.csproj