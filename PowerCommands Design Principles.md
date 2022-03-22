# POWERCOMMANDS ARCHITECTURE AND DESIGN PRINCIPLES
# Components
## Main components
 - PowerCommand Client
 - Bootstrap component
 - Third party component
 - Core component
 - Custom components
 - Reusable Commands
 - PowerCommands

 ![Alt text](PowerCommand_component_diagram.png?raw=true "Title")

 ## PowerCommand Console
 ### Keep the Console appliakation as clean as possible
 The console application should be used as is so that the look and feel of Power Commands is consistent, it is nicer when using a combination of PowerCommands grouped togher in one directory sharing the same configuration where you could change font color and background color and other core behaivours.
 ## Bootstrap component
 The bootstrap component is the glue between the Console and the other modules, it has a Startup class whos purpose is to initialize the application, in the bootstrap component you could choose a your prefered logging library, you could overide default settings and do other customization setup.

## Extend PowerCommand
### Avoid changes in the PowerCommand Core, extend instead
 PowerCommand Framework is distributed as a Nuget Package and as open source code at Github, if you use the source code, avoid to change anything extend the functionallity instead if you feel that you need to do that. That way it is easier for you when something has changed in the Core.

# SECURITY
## Always encrypt secrets 
### Use EncryptManager
 PowerCommands has encryption built in, see patterns in the Developer Guidelines documentation
### Store secrets outside the application path
 PowerCommands handles export and imports of environment variables using YAML files it is the preferred way to store secrets. Dont store sensitive information inside the application path as it will be to easy to steal with a simple copy and paste operation.

# CONFIGURATION
## Use YAML
 PowerCommands has support for reading and writing object as YAML and that format should always be used for configuration files. 
## Configuration could be shared by clients
The main configuration should be named powercommand.yaml and be a valid YAML file, it could be shared by many clients residing in the same directory.

# Documentation
## Use Markdown format
 The format of dokumentation in text should use the markdown format.
## Always describe your PowerCommands
 Be kind to your consumer as it often turn out to be you that are the consumer, fill in a shourt description of what the PowerCommand is doing and how to use it.
## Use [Desription] attribute
