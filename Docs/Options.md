# Options

Options is what the name implies, options for your command, lets have a look at this example below.
![Alt text](images/power_command_attribute.png?raw=true "Attributes")

You could also se that this Command has two options separated with pipe **"path|format"**, first one named **path** and the other one is **format**. This will give the user autocomplete feedback when typing - and using the [Tab] tangent.

Programatically you can use Options in two ways, you could grab the value, wich is the parameter typed after the option, like this (using the RegressionCommand as example, it is a user created command).

``` convert --path "C:\temp\test.yaml" --format json```

To get the value of the **mode** option you code like this:

``` var path = input.GetOptionValue("path"); ```

Sometimes you just want a option without a value, you can solve that like this:

``` var encrypt = input.HasOption("encrypt"); ```

## The CommandBase class holds a list of the declared options with the input value
That is another way to programatically work with the options.

### Do not use the help option, unless you want to override it´s behaviour
It will not harm anything but --help will trigger the Core frameowork to display generic help (using the PowerCommandDesign attriute).
You can override this behaviour if you set the property **overrideHelpOption** to true. Do not do that if your not intend to implement your own show help functionalllity for that command.

Read more about:

[Design your Command](Design_command.md)

[Input](Input.md)

[PowerCommands Attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)