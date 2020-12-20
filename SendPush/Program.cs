using System;
using Afarazit.Settings;
using PushoverClient;

namespace SendPush
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Get the settings defaults
            string appKey = AppSettings.GetKey("appKey");
            string userGroupKey = AppSettings.GetKey("userGroupKey");

            //  Get the command line options
            Options options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //  If we didn't get the app key passed in, use the default:
                if (string.IsNullOrEmpty(options.From))
                {
                    options.From = appKey;
                }

                //  If we didn't get the user key passed in, use the default:
                if (string.IsNullOrEmpty(options.User))
                {
                    options.User = userGroupKey;
                }

                //  Make sure we have our required items:
                if (OptionsValid(options))
                {
                    //  Send the message
                    Pushover pclient = new Pushover(options.From);
                    PushResponse response = pclient.Push(options.Title, options.Message, options.User);
                }
                else
                    Console.WriteLine(options.GetUsage());

            }
        }

        static bool OptionsValid(Options options)
        {
            bool retval = false;

            if (!string.IsNullOrEmpty(options.From) &&
                !string.IsNullOrEmpty(options.User) &&
                !string.IsNullOrEmpty(options.Title) &&
                !string.IsNullOrEmpty(options.Message))
            {
                retval = true;
            }

            return retval;
        }
    }
}