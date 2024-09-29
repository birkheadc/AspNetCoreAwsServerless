variable "user_pool_name" {
  type = string
}

variable "email_arn" {
  type = string
}

variable "frontend_url" {
  type = string
}

variable "is_development" {
  type = bool
}

variable "localhost_port" {
  type    = string
  default = "5173"
}
