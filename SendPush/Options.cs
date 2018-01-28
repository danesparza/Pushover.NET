using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace SendPush
{
    public class Options
    {
        [Option('t', "title", HelpText = "The message title")]
        public string Title
        {
            get;
            set;
        }

        [Option('m', "message", HelpText = "The message")]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// The Pushover.NET api key to send the message from
        /// </summary>
        [Option('f', "from", HelpText = "The Pushover api key to send the message from")]
        public string From
        {
            get;
            set;
        }

        /// <summary>
        /// The Pushover.NET api key to send the message to
        /// </summary>
        [Option('u', "user", HelpText = "The Pushover api key to send the message to")]
        public string User
        {
            get;
            set;
        }

        [HelpOption('?', "help", HelpText = "Show this help screen")]
        public string GetUsage()
        {
            var help = new HelpText
            {
                Heading = new HeadingInfo("SendPush", "0.2"),
                Copyright = new CopyrightInfo("Dan Esparza", 2013),
                AddDashesToOption = true
            };

            help.AddOptions(this);

            return help;
        }
    }
}
