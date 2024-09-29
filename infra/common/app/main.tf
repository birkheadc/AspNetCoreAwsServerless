module "cognito" {
  source         = "../modules/cognito"
  user_pool_name = "${var.app_name}-${var.env_name}"
  email_arn      = var.email_arn
  frontend_url   = var.frontend_url
  is_development = var.env_name == "Development"
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
  source           = "../modules/iam_policies"
  books_table_name = aws_dynamodb_table.books_table.name
  users_table_name = aws_dynamodb_table.users_table.name
  app_name         = var.app_name
  stage_name       = var.env_name
}

module "api_lambda_function" {
  source        = "../modules/lambda_function"
  bucket_id     = aws_s3_bucket.lambda_bucket.id
  publish_dir   = "${path.module}/../../../src/${var.publish_directory_path}"
  zip_file      = "build.zip"
  function_name = "${var.app_name}_${var.env_name}"
  handler       = var.lambda_handler

  environment_variables = {
    ASPNETCORE_ENVIRONMENT          = var.env_name
    ASPNETCORE_COGNITO_USER_POOL_ID = module.cognito.user_pool_id
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
  source     = "../modules/api_gateway"
  api_name   = "${var.app_name}_Api"
  stage_name = var.env_name
}

module "api_gateway_lambda_integration" {
  source        = "../modules/api_gateway_lambda_integration"
  api_id        = module.api_gateway.api_id
  api_arn       = module.api_gateway.api_arn
  function_arn  = module.api_lambda_function.function_arn
  function_name = module.api_lambda_function.function_name
}


