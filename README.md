# C\# Message Passing

This repository contains an example of message passing in C# based on the
asynchronous example by Microsoft.

## Included Projects

The `Breakfast` project contains the types Microsoft referenced within their
example.

The `AsyncBreakfast` project contains [the Microsoft example](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/async/#await-tasks-efficiently).

The `MessageBreakfast` project contains the message-passing example with
identical output to `AsyncBreakfast`.

The `MPL` project contains the message-passing library; it manages the runtime
concurrency and the primitives necessary to interact with it.
