variable "prefix" {
  description = "The prefix used for all resources in this example"
  default = "grafana"
}

variable "location" {
  description = "The Azure location where all resources in this example should be created"
  default = "eastus2"
}

variable "app_name" {
  default = "rcpigrafana"
}