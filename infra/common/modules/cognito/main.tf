resource "aws_cognito_user_pool" "this" {
  name              = var.user_pool_name
  mfa_configuration = "OPTIONAL"
  software_token_mfa_configuration {
    enabled = true
  }

  account_recovery_setting {
    recovery_mechanism {
      name     = "verified_email"
      priority = 1
    }
  }

  auto_verified_attributes = ["email"]
  username_attributes      = ["email"]

  user_attribute_update_settings {
    attributes_require_verification_before_update = ["email"]
  }

  schema {
    attribute_data_type      = "String"
    name                     = "email"
    developer_only_attribute = false
    mutable                  = false
    required                 = true
  }

  email_configuration {
    from_email_address    = "Colby Birkhead <mail@birkheadc.me>"
    email_sending_account = "DEVELOPER"
    source_arn            = var.email_arn
  }
}

resource "aws_cognito_user_pool_domain" "this" {
  user_pool_id = aws_cognito_user_pool.this.id
  domain       = lower(var.user_pool_name)
}

resource "aws_cognito_user_pool_client" "this" {
  name         = "${var.user_pool_name}-Client"
  user_pool_id = aws_cognito_user_pool.this.id

  callback_urls = flatten(["${var.frontend_url}/login", var.is_development ? ["http://localhost:${var.localhost_port}/login"] : []])

  supported_identity_providers = ["COGNITO"]

  allowed_oauth_flows_user_pool_client = true

  allowed_oauth_flows  = ["code"]
  allowed_oauth_scopes = ["email", "openid", "aws.cognito.signin.user.admin"]

  explicit_auth_flows = ["ALLOW_REFRESH_TOKEN_AUTH", "ALLOW_USER_PASSWORD_AUTH", "ALLOW_USER_SRP_AUTH"]
}
