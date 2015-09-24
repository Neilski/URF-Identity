# URF-Identity
An ASP.net 2.2 implementation using the Generic Unit of Work and Repositories Framework

This project uses Long Le's elegant [Unit of Work & (extensible/generic) Repositories Framework](http://genericunitofworkandrepositories.codeplex.com/) to manage the Data Access Layer for Microsoft's ASP.NET Identity 2.2.1 implementation.

The project was built from the standard Visual Studio MVC application with external class libraries used to define:

* The POCO application models (including the customised Identity models)
* The Generic Unit of Work and Repositories Framework
* The customised ASP.NET Identity implementation
* The Data Access Layer (DAL)

## Basic Setup

To get started, I would suggest that you have a quick review of the following files in the UrfIdentity.Web project:

1. /Web.config.  You'll probably want to define your SMTP service and (optionally) configure the Twilio (for 2FA SMS) and required OAuth providers (Google, Facebook, Microsoft, Twitter, LinkedIn etc.) in the appSettings section.
2. /Startup.cs and /Global.asx.cs to check initialisation wiring
3. /App_Start/Startup.ConfigureAuth.cs for Identity and OAuth Provider configuration
4. /App_Start/Startup.ConfigureContainer for Autofac (IoC/DI) configuration

That should be all you need to run the project (if you get database creation errors you might also need to rename the database in the Web.config ConnectionString definition).

Once you have it running, you will probably want to play with the way the Identity work flow is configured - just look at the code and comments in the /Controllers/AccountController.cs.