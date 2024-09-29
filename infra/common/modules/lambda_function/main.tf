data "archive_file" "archive" {
  type        = "zip"
  source_dir  = var.publish_dir
  output_path = "${path.module}/../../${var.zip_file}"
}

resource "aws_s3_object" "bundle" {
  bucket = var.bucket_id
  key    = var.zip_file
  source = data.archive_file.archive.output_path
  etag   = filemd5(data.archive_file.archive.output_path)
}

resource "aws_lambda_function" "function" {
  function_name    = var.function_name
  s3_bucket        = var.bucket_id
  s3_key           = aws_s3_object.bundle.key
  runtime          = "dotnet8"
  handler          = var.handler
  source_code_hash = data.archive_file.archive.output_base64sha256
  role             = aws_iam_role.function_role.arn
  timeout          = 30
  memory_size      = 256
  dynamic "environment" {
    for_each = length(var.environment_variables) > 0 ? [1] : []
    content {
      variables = var.environment_variables
    }
  }
}

resource "aws_cloudwatch_log_group" "aggregator" {
  name = "/aws/lambda/${aws_lambda_function.function.function_name}"

  retention_in_days = 14
}

resource "aws_iam_role" "function_role" {
  name               = "FunctionIamRole_${var.function_name}"
  assume_role_policy = data.aws_iam_policy_document.lambda_assume_role_policy.json
}

resource "aws_iam_role_policy_attachment" "policy_attachment" {
  role       = aws_iam_role.function_role.name
  policy_arn = "arn:aws:iam::aws:policy/service-role/AWSLambdaBasicExecutionRole"
}
