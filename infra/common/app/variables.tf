variable "app_name" {
  description = "Name of this application"
  type        = string
}

variable "publish_directory_path" {
  description = "The path to the public directory (after the src folder)"
  type        = string
}

variable "env_name" {
  description = "Environment name"
  type        = string
}

variable "region" {
  description = "The region to deploy the application"
  type        = string
  default     = "ap-southeast-2"
}

variable "email_arn" {
  description = "The ARN of the email address used to send verification email etc. with Cognito"
  type        = string
}

variable "frontend_url" {
  description = "The root URL for the frontend, used as a callback for Cognito"
  type        = string
}

variable "lambda_handler" {
  description = "The name of the lambda handler created by Dotnet"
  type        = string
}
