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

## Update - Major Change
There was a bug in the original version of this code that prevented changes made via the Identity 2.0 methods to be persisted in the database.  In summary, Identity 2.0 know nothing about the URF's IObjectState management so the URF DataContext's SaveChanges() and SaveChangesAsync() methods could not correctly detect and management the entity state (because IObjectState was left 'unchanged').

The approach taken to address this problem (there may well be other/better solutions!) was to override the Repository.Pattern.Ef6.DataContext class and insert a new virtual method called from the body of the SyncObjectsStatePreCommit() method.  This virtual method is then overridden in the final Application DataContext to look for and manage Identity 2.0 based entities differently to native URF derived entities.
