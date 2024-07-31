output "api_id" {
  value = aws_apigatewayv2_api.lambda_api.id
}

output "api_arn" {
  value = aws_apigatewayv2_api.lambda_api.execution_arn
}
