# Application - Event Manager

The Event Management System allows organizations to create, manage, and host events.
Users can browse available events, subscribe to receive notifications for events, keep track of upcoming events.
Event organizers can configure event details such as locations, categories and tags.
Administration can create, edit, and assign permissions through the system. They can also
oversee the platform, manage users, handle global configurations.

## Setting up the development environment

You will need the following:
 - SQL Server
 - .NET 8
 
Withing sql server, create 2 databases, with the following names:
 - EventManager
 - EventManager_Hangfire
 
Update appsettings.json to point to your local database

Run the application.

You can login with the pre-created user:
Mail: admin@gmail.com
Pass: admin