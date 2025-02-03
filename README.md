# Data Processor Exercise
## Objectives

This exercise shows some asynchronous programming concepts, including converting methods to `async`, letting `async` flow through the application.

## Overview

Open the `DataProcessor.sln` solution.

This solution is a console application that processes data from a text file. The `DataLoader` class creates a list of strings from the contents of the file `data.txt`. These strings are passed to the `DataParser` class. The `DataParser` is responsible for parsing the strings into `Person` objects (which are then displayed to the console). Errors are logged to a file using the `FileLogger` class.

Run the application by pressing F5. This will build the application and give us some output that we can ignore for now.

In File Explorer, open the output folder: *[working_directory]/DataProcessor/bin/Debug/net8.0/*. Open the `data.txt` file. This contains a number of comma-separated value (CSV) records along with some invalid records. Run the application by double-clicking "DataProcessor.exe". The output will be as follows:

```
Successfully processed 9 records
John Koenig
Dylan Hunt
Turanga Leela
John Crichton
Dave Lister
Laura Roslin
John Sheridan
Dante Montana
Isaac GampuCopy to clipboardErrorCopied
```

Now open the "log.txt" file in the same folder. It contains the errors and bad records.

```
==================
2022-05-26T12:38:33: Wrong number of fields in record - INVALID RECORD FORMAT
2022-05-26T12:38:33: Cannot parse Start Date field - 20,Check,Date,0/2//,9,{1} {0}
2022-05-26T12:38:33: Wrong number of fields in record - BAD RECORD
2022-05-26T12:38:33: Cannot parse Rating field - 21,Check,Rating,2014/05/03,a,Copy to clipboardErrorCopied
```

## Objective

Our goal is to change the logging function to an asynchronous operation. Our logger can then access files asynchronously. In addition, we will allow the async operations to propagate through the application.

## Classes

**DataProcessor.Library/ILogger**
This is the interface for logging:

```csharp
public interface ILogger
{
    void LogMessage(string message, string data);
}Copy to clipboardErrorCopied
```

**DataProcessor.Library/FileLogger**
Implements the `ILogger` interface to log messages to a file. Here is the `LogMessage` implementation:

```c#
public void LogMessage(string message, string data)
{
    using var writer = new StreamWriter(logPath, true);
    writer.WriteLine(
        $"{DateTime.Now:s}: {message} - {data}");
}Copy to clipboardErrorCopied
```

**DataProcessor.Library/DataParser**
Parses the data file and uses the `ILogger` interface to log any errors. The logger is passed in through the constructor.

```csharp
public DataParser(ILogger logger)
{
    this.logger = logger ?? new NullLogger();
}Copy to clipboardErrorCopied
```

Note: if a logger is not passed in, then a `NullLogger` is used. This is a logger that does nothing.

The logger is used in the `ParseData` method of the `DataParser` class. We will look at this method a bit more closely when making changes.

**DataProcessor/Program**
The program class has the entry point for the console application. Here is the `ProcessData` method:

```csharp
static IReadOnlyCollection<Person> ProcessData()
{
    var loader = new DataLoader();
    IReadOnlyCollection<string> data = loader.LoadData();

    var logger = new FileLogger();
    var parser = new DataParser(logger);
    var records = parser.ParseData(data);
    return records;
}Copy to clipboardErrorCopied
```

This method loads the data from the text file, then creates the logger and data parser classes. It then calls `ParseData` and returns the records that come back.

## Hints

Remember that our goal is to change the logging function to an asynchronous operation. Here are some hints that will guide you.

In `ILogger.cs`:

- Change the `LogMessage` interface to return `Task` instead of `void`.

In `FileLogger.cs`:

- Change `LogMessage` to an asynchronous method to match the interface.
- The `StreamWriter` class has an asynchronous `WriteLineAsync` method that can be used.

In `NullLogger.cs`:

- Update the class to satisfy the updated `ILogger` interface.

- Do not return `null`from the method. There is a static property on `Task` that can be used instead.

  

In `DataParser.cs`:

- Await the `logger.LogMessage` calls in the `ParseData` method.
- Let the `async`bubble up through this method.

In `Program.cs`:

- Update the `ProcessData` method as needed. (That is all of the hints you get for this one).

