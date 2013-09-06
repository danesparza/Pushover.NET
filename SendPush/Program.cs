using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PushoverClient;

namespace SendPush
{
    class Program
    {
        static void Main(string[] args)
        {
            //  Get the settings defaults
            string appKey = ConfigurationManager.AppSettings["appKey"];
            string userGroupKey = ConfigurationManager.AppSettings["userGroupKey"];

            //  Get the command line options
            Options options = new Options();
            if(CommandLine.Parser.Default.ParseArguments(args, options))
            {
                //  If we didn't get the app key passed in, use the default:
                if(string.IsNullOrEmpty(options.From))
                {
                    options.From = appKey;
                }

                //  If we didn't get the user key passed in, use the default:
                if(string.IsNullOrEmpty(options.User))
                {
                    options.User = userGroupKey;
                }

                //  Send the message
                Pushover pclient = new Pushover(options.From);
                PushResponse response = pclient.Push(options.Title, options.Message, options.User);
            }
        }
    }
}
