#!/bin/bash
echo "Starting API..."
ASPNETCORE_ENVIRONMENT=Development dotnet watch --project src/AspNetCoreAwsServerless/AspNetCoreAwsServerless.csproj