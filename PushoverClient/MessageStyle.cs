using System;
using System.Collections.Generic;
using System.Linq;
namespace PushoverClient
{
    //Detail at https://pushover.net/api#html
    public enum MessageStyle
    {
        NotSet, //Default = will leave empty on sending
        html,
        monospace
    }
}