# URF-Identity
An ASP.net 2.2 implementation using the Generic Unit of Work and Repositories Framework

This project uses Long Le's elegant [Unit of Work & (extensible/generic) Repositories Framework](http://genericunitofworkandrepositories.codeplex.com/) to manage the Data Access Layer for Microsoft's ASP.NET Identity 2.2.1 implementation.

The project was built from the standard Visual Studio MVC application with external class libraries used to define:

* The POCO application models (including the customised Identity models)
* The Generic Unit of Work and Repositories Framework
* The customised ASP.NET Identity implementation
* The Data Access Layer (DAL)

## WARNING - This project modifies the standard URF Library Package
Through testing and feedback, it has become apparent that the most reliable way of coercing Microsoft's ASP.NET Identity to work reliably with the URF is to re-engineer the Repository.Pattern.Ef6.DataContext.SyncObjectsStatePreCommit() method.  *This is not an ideal solution*, but does appear to address the problem of Identity's inner methods being unaware of the URF's requirement to correctly set the ObjectState.

***Note, this code is somewhat experimental at this stage - so please validate carefully to make sure it works in your particular application***

## Basic Setup

To get started, I would suggest that you have a quick review of the following files in the UrfIdentity.Web project:

1. /Web.config.  You'll probably want to define your SMTP service and (optionally) configure the Twilio (for 2FA SMS) and required OAuth providers (Google, Facebook, Microsoft, Twitter, LinkedIn etc.) in the appSettings section.
2. /Startup.cs and /Global.asx.cs to check initialisation wiring
3. /App_Start/Startup.ConfigureAuth.cs for Identity and OAuth Provider configuration
4. /App_Start/Startup.ConfigureContainer for Autofac (IoC/DI) configuration

That should be all you need to run the project (if you get database creation errors you might also need to rename the database in the Web.config ConnectionString definition).

Once you have it running, you will probably want to play with the way the Identity work flow is configured - just look at the code and comments in the /Controllers/AccountController.cs.


## The Problem with Identity and URF
Around 90% of Identity's functionality can be achieved without modifying the URF library - see commented code in the Identity.ApplicationUserStore and Identity.ApplicationRoleStore.  Unfortunately, where Identity manages sub-properties of the user (role assignment for example), this is handled within the Identity UserStore implementation itself (see the Identity Source code for details), so there does not appear to be any reliable way of updating the UserRole ObjectState within the application's Identity overrides - at least not without creating a full URF implementation of the Identity interfaces.  Whilst this is possible, it does add a significant overhead in development and testing.