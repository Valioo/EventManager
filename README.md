# Application - Event Manager

The Event Management System allows organizations to create, manage, and host events.
Users can browse available events, subscribe to receive notifications for events, keep track of upcoming events.
Event organizers can configure event details such as locations, categories and tags.
Administration can create, edit, and assign permissions through the system. They can also
oversee the platform, manage users, handle global configurations.

## Setting up the development environment

You will need the following:
 - Docker
 - .NET 8
 - tflocal
 
To start the containers, execute:
```
docker-compose up -d
```

Application is running localstack instead of connecting to AWS infra. In order to set up the container within /terraform execute the following:
```
tflocal init
tflocal plan
tflocal apply
```

Run the application.

You can login with the pre-created user:
Mail: admin@gmail.com
Pass: admin

## Creating migrations

To create a migration, execute the following:

```bash
dotnet ef migrations add {Name} --project EventManager.Domain --startup-project EventManager.API
```