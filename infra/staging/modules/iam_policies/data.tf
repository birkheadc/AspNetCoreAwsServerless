data "aws_caller_identity" "current" {

}

data "aws_region" "current" {

}

data "aws_iam_policy_document" "dynamo_db_full_access" {
  statement {
    actions = ["*"]
    resources = [
      "arn:aws:dynamodb:*:${data.aws_caller_identity.current.account_id}:table/${var.books_table_name}",
      "arn:aws:dynamodb:*:${data.aws_caller_identity.current.account_id}:table/${var.books_table_name}/*",
      "arn:aws:dynamodb:*:${data.aws_caller_identity.current.account_id}:table/${var.users_table_name}",
      "arn:aws:dynamodb:*:${data.aws_caller_identity.current.account_id}:table/${var.users_table_name}/*",
    ]
  }
}

data "aws_iam_policy_document" "cloudwatch_put_metrics" {
  statement {
    actions   = ["cloudwatch:PutMetricData"]
    resources = ["*"]
  }
}
