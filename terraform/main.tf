provider "aws" {
  region                      = "us-east-1"
  access_key                  = "test"
  secret_key                  = "test"
  skip_credentials_validation = true
  skip_requesting_account_id  = true
  endpoints {
    sns = "http://localhost:4566"
  }
}

resource "aws_sns_topic" "event_notifications" {
  name = "event-notifications"
}

output "sns_topic_arn" {
  value = aws_sns_topic.event_notifications.arn
}