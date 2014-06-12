Pushover.NET
============

.NET Wrapper for the [Pushover](http://pushover.net) API.  Pushover makes it easy to send real-time notifications to your Android and iOS devices.

### Quick Start

Install the [NuGet package](http://www.nuget.org/packages/PushoverNET/)
```powershell
Install-Package PushoverNET
```

Next, you will need to provide Pushover.NET with your API key in code.  Need help finding your API key?  Check here: https://pushover.net/faq

In your application, call:

```CSharp
Pushover pclient = new Pushover("Your-apps-API-Key-here");
PushResponse response = pclient.Push(
              "Test title", 
              "This is the test message.", 
              "User-key-to-send-to-here"
          );
```

### Examples

##### Pushing a message:

```CSharp
using PushoverClient;

namespace ConsoleApplication1
{
  class Program
  {
      static void Main(string[] args)
      {
          Pushover pclient = new Pushover("Your-apps-API-Key-here");

          PushResponse response = pclient.Push(
              "Test title", 
              "This is the test message.", 
              "User-key-to-send-to-here"
          );
      }
  }
}
```


