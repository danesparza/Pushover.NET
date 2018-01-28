using System;
using PushoverClient;
using Microsoft.Extensions.Configuration;

namespace SendPush
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(string.Join(",",args));
            // setup configuration & User Secrets
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            //  Get the settings defaults
            var appKey = configuration["Pushover_Token"];
            var userGroupKey = configuration["Pushover_UserId"];


            //  Get the command line options
            var options = new Options();
            var result = CommandLine.Parser.Default.ParseArguments(args, options);
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

            Console.WriteLine(options.From);
            Console.WriteLine(options.User);
            Console.WriteLine(options.Title);
            Console.WriteLine(options.Message);

            //  Make sure we have our required items:
            if (!OptionsValid(options))
            {
                Console.WriteLine(options.GetUsage());
                return;
            }

            //  Send the message
            var pclient = new Pushover(options.From);
            var response = pclient.Push(options.Title, options.Message, options.User);
        }

        private static bool OptionsValid(Options options)
        {
            return !string.IsNullOrEmpty(options.From)
                && !string.IsNullOrEmpty(options.User)
                && !string.IsNullOrEmpty(options.Title)
                && !string.IsNullOrEmpty(options.Message);
        }
    }
}
