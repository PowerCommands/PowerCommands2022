# Flags

This image below shows a nice description of the command with the built in describe command.
![Alt text](images/attributes.png?raw=true "Attributes")

You could also se that this Command has two flags separated with pipe **"mode|name"**, first one named **mode** and the other one is **name**. This will give the user autocomplete feedback when typing - and using the [Tab] tangent.

Programatically you can use Flags in two ways, you could grab the value, wich is the parameter typed after the flag, like this (using the RegressionCommand as example, it is a user created command).

``` regression --mode normal --name "My sample project" ```

To get the value of the **mode** flag you code like this:

``` var mode = input.GetFlagValue("mode"); ```

Sometimes you just want a flag without a value, you can solve that like this:

``` var mode = input.HasFlag("xml"); ```

### Do not use the help flag, unless you want to override it´s behaviour
It will not harm anything but --help will trigger the Core frameowrk to display generic help (using the PowerCommands attriute).
You can override this behaviour if you set the property **overrideHelpFlag** to true. Do not do that if your not intend to implement your own show help functionalllity for that command.

Read more about:

[Design your Command](Design_command.md)

[Input](Input.md)

[PowerCommands Attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)