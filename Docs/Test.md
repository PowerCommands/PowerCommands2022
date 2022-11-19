# Test

To make your command testable by the Core runtime you only need to add the PowerCommandTest attribute to your command class like this.

```
[PowerCommandTest(tests: " |--this|--reserved|\"encrypt\"|--default")]
```
The example above shows the test that are declared on the core command class **Commands** command, the syntax is really simple.

For every test you want to run, just add the parameterlist to property **tests**, the above declaration will genereate five test with this commands.

**commands**

**commands** --this

**commands** --reserved

**commands** "encrypt"

**commands** --default

To run the test just type this command:

**Test** --command commands

And this will be the result:

![Alt text](images/test_result.png?raw=true "Test result")

## Test bad result

That was happy path testing but sometimes you want to test for expected bad results also. Lets say that you have a option that must have a value if it is used and you want test that this validation really work, you want to trigger a error. If a error occurs you are happy, the test is a success. 

How do you do that?

That is simple, you youst put a **!** character first, like in this example that is used on the **option** command.
```
[PowerCommandTest(        tests: "--mandatory value|!--optional someValue --mandatory")]
[PowerCommandDesign(description: "Try out how option works, first option requires a value, second does not.",
                          options: "!mandatory|optional")]
```
Notice that the first option mandatory has a trailing **!** character wich means that if this option is used it must have a value otherwise the validation will fail.
The test reflecs that constraint, the second thest will run the option command like this:

```option --optional someValue --mandatory```

The mandatory option has no value and validation will fail, wich the test also expects, this is the result when running the test command.

```test --command option```

![Alt text](images/test_option.png?raw=true "Test result")

## Run test for all commands
Usually I think you want to run through all your commands that has declared as testable using the PowerCommandTest attribute, just run this command:

```test --all```

And you may get something like this:

![Alt text](images/test_all.png?raw=true "Test result")

## Be awere the command runs "for real" and async command should be excluded
The test will run the command as you had started them normally, if a command uses a dialog you will be prompted and must answer it.
Async commands does not work weil as the result is a bit unpredictable.

Read more about:

[Design your Command](Design_command.md)

[Input](Input.md)

[PowerCommands Attribute](PowerCommandAttribute.md)

[Back to start](https://github.com/PowerCommands/PowerCommands2022/blob/main/Docs/README.md)