resource "aws_iam_policy" "dynamo_db_full_access" {
  name   = "dynamo_db_full_access_policy"
  path   = "/"
  policy = data.aws_iam_policy_document.dynamo_db_full_access.json
}

resource "aws_iam_policy" "cloudwatch_put_metrics" {
  name   = "cloudwatch_put_metrics"
  path   = "/"
  policy = data.aws_iam_policy_document.cloudwatch_put_metrics.json
}
