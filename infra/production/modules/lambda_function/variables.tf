variable "bucket_id" {
  type = string
}

variable "publish_dir" {
  type = string
}

variable "zip_file" {
  type = string
}

variable "function_name" {
  type = string
}

variable "handler" {
  type = string
}

variable "environment_variables" {
  type = map(string)
}
