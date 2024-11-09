output "client_id" {
  value = aws_cognito_user_pool_client.this.id
}

output "url" {
  value = "https://${aws_cognito_user_pool_domain.this.domain}.auth.${var.region}.amazoncognito.com"
}
