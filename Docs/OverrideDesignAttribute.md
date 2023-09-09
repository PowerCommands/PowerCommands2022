# Override PowerCommandsDesign Attribute
## Edit PowerCommandsConfiguration.yaml file
In some cases you may want to override the **PowerCommandsDesignAttribute** a typical use case could be that you want a different kind of suggestions or add suggestions to a certain command in your implementation but you do not want to change the source code and recompile it.
Then this feature to override it in configuration comes in handy. 

![Alt text](images/override_design_attribute.png?raw=true "Attribute")

The image above illustrates how it could look like, the table below shows those properties of the design attribute you can override.
### PLEASE NOTE
In the configuration file you just add those properties you want to override, the **name** property is not a property of the design attribute, it is the name of the command that you are overriding the design attribute for.

| Property Name    | Data Type |
|------------------|-----------|
| **name**         | string    |
| description      | string    |
| arguments        | string    |
| quotes           | string    |
| options          | string    |
| examples         | string    |
| suggestions      | string    |
| useAsync         | boolean   |
| showElapsedTime  | boolean   |

### Read more about:

[PowerCommandDesign Attribute](PowerCommandDesignAttribute.md)

[Design your Command](Design_command.md)

[Input](Input.md)

[options](options.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)