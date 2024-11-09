output "client_id" {
  value = aws_cognito_user_pool_client.this.id
}

output "url" {
  value = aws_cognito_user_pool.this.endpoint
}
