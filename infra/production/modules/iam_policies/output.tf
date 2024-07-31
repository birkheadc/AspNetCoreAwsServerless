output "dynamo_db_full_access" {
  value = aws_iam_policy.dynamo_db_full_access.arn
}

output "cloudwatch_put_metrics" {
  value = aws_iam_policy.cloudwatch_put_metrics.arn
}
