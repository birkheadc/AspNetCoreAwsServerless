resource "aws_iam_policy" "dynamo_db_full_access" {
  name   = "DynamoDbFullAccess_${var.app_name}_${var.stage_name}"
  path   = "/"
  policy = data.aws_iam_policy_document.dynamo_db_full_access.json
}

resource "aws_iam_policy" "cloudwatch_put_metrics" {
  name   = "CloudwatchPutMetrics_${var.app_name}_${var.stage_name}"
  path   = "/"
  policy = data.aws_iam_policy_document.cloudwatch_put_metrics.json
}

resource "aws_iam_policy" "ssm_data_protection" {
  name   = "SSMDataProtection_${var.app_name}_${var.stage_name}"
  path   = "/"
  policy = data.aws_iam_policy_document.ssm_data_protection.json
}
