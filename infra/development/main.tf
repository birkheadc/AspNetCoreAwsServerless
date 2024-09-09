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

resource "aws_dynamodb_table" "books_table" {
  name         = "${var.app_name}_${var.env_name}_Books"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "Id"

  attribute {
    name = "Id"
    type = "S"
  }
}

resource "aws_dynamodb_table" "users_table" {
  name         = "${var.app_name}_${var.env_name}_Users"
  billing_mode = "PAY_PER_REQUEST"
  hash_key     = "Id"

  attribute {
    name = "Id"
    type = "S"
  }
}

resource "aws_s3_bucket" "lambda_bucket" {
  bucket        = "${lower(var.app_name)}-${lower(var.env_name)}-lambda-bucket"
  force_destroy = true
}

module "iam_policies" {
  source     = "./modules/iam_policies"
  table_name = aws_dynamodb_table.books_table.name
  app_name   = var.app_name
  stage_name = var.env_name
}

module "api_lambda_function" {
  source        = "./modules/lambda_function"
  bucket_id     = aws_s3_bucket.lambda_bucket.id
  publish_dir   = "${path.module}/../../src/${var.app_name}/bin/Release/net8.0/linux-x64/publish"
  zip_file      = "build.zip"
  function_name = "${var.app_name}_${var.env_name}"
  handler       = "${var.app_name}::${var.app_name}.LambdaEntryPoint::FunctionHandlerAsync"

  environment_variables = {
    ASPNETCORE_ENVIRONMENT = var.env_name
  }
}

resource "aws_iam_role_policy_attachment" "api_lambda_dynamodb_full_access" {
  role       = module.api_lambda_function.function_role_name
  policy_arn = module.iam_policies.dynamo_db_full_access
}

resource "aws_iam_role_policy_attachment" "api_lambda_cloudwatch_metrics" {
  role       = module.api_lambda_function.function_role_name
  policy_arn = module.iam_policies.cloudwatch_put_metrics
}

module "api_gateway" {
  source     = "./modules/api_gateway"
  api_name   = "${var.app_name}_Api"
  stage_name = var.env_name
}

module "api_gateway_lambda_integration" {
  source        = "./modules/api_gateway_lambda_integration"
  api_id        = module.api_gateway.api_id
  api_arn       = module.api_gateway.api_arn
  function_arn  = module.api_lambda_function.function_arn
  function_name = module.api_lambda_function.function_name
}
