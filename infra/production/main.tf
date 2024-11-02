terraform {
  backend "s3" {}
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = ">= 5.59"
    }
  }

  required_version = ">= 1.9.2"
}

provider "aws" {
  region = "ap-southeast-2"
}

module "app" {
  source                 = "../common/app"
  app_name               = "AspNetCoreServerless"
  env_name               = "Production"
  email_arn              = "arn:aws:ses:ap-southeast-2:290153383648:identity/birkheadc.me"
  publish_directory_path = "AspNetCoreAwsServerless/bin/Release/net8.0/linux-x64/publish"
  frontend_url           = "https://vite-template.birkheadc.me"
  lambda_handler         = "AspNetCoreAwsServerless::AspNetCoreAwsServerless.LambdaEntryPoint::FunctionHandlerAsync"
  allowed_origins        = ["https://vite-template.birkheadc.me"]
}
