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
  source   = "../common/app"
  app_name = "AspNetCoreAwsServerless"
  env_name = "Staging"
}
