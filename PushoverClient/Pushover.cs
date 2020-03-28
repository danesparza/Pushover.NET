using ServiceStack;
using System;
using System.Net;
using System.Threading.Tasks;

namespace PushoverClient
{
    /// <summary>
    /// Client library for using Pushover for push notifications.  
    /// See https://pushover.net/api for more information
    /// </summary>
    public class Pushover
    {
        /// <summary>
        /// Base url for the API
        /// </summary>
        private const string BASE_API_URL = "https://api.pushover.net/1/messages.json";

        /// <summary>
        /// The application key
        /// </summary>
        public string AppKey
        {
            get;
            set;
        }

        /// <summary>
        /// The default user or group key to send messages to
        /// </summary>
        public string DefaultUserGroupSendKey
        {
            get;
            set;
        }

        /// <summary>
        /// Create a pushover client using a source application key.
        /// </summary>
        /// <param name="appKey"></param>
        public Pushover(string appKey)
        {
            AppKey = appKey;
        }

        /// <summary>
        /// Create a pushover client using both a source 
        /// application key and a default send key
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="defaultSendKey"></param>
        public Pushover(string appKey, string defaultSendKey) : this(appKey)
        {
            DefaultUserGroupSendKey = defaultSendKey;
        }

        /// <summary>
        /// Push a message
        /// </summary>
        /// <param name="title">Message title</param>
        /// <param name="message">The body of the message</param>
        /// <param name="userKey">The user or group key (optional if you have set a default already)</param>
        /// <param name="device">Send to a specific device</param>
        /// <param name="priority">Priority of the message (optional) default value set to Normal</param>
        /// <param name="notificationSound">If set sends the notification sound</param>
        /// <returns></returns>
        public PushResponse Push(string title, string message, string userKey = "", string device = "", Priority priority = Priority.Normal, DateTime? timestamp = null, NotificationSound notificationSound = NotificationSound.NotSet)
        {
            var args = CreateArgs(title, message, userKey, device, priority, timestamp ?? DateTime.UtcNow, notificationSound);
            try
            {
                return BASE_API_URL.PostToUrl(args).FromJson<PushResponse>();
            }
            catch (WebException webEx)
            {
                return webEx.GetResponseBody().FromJson<PushResponse>();
            }
        }

        /// <summary>
        /// Push a message
        /// </summary>
        /// <param name="title">Message title</param>
        /// <param name="message">The body of the message</param>
        /// <param name="userKey">The user or group key (optional if you have set a default already)</param>
        /// <param name="device">Send to a specific device</param>
        /// <param name="priority">Priority of the message (optional) default value set to Normal</param>
        /// <param name="notificationSound">If set sends the notification sound</param>
        /// <returns></returns>
        public async Task<PushResponse> PushAsync(string title, string message, string userKey = "", string device = "", Priority priority = Priority.Normal, DateTime? timestamp = null, NotificationSound notificationSound = NotificationSound.NotSet)
        {
            var args = CreateArgs(title, message, userKey, device, priority, timestamp?? DateTime.UtcNow, notificationSound);
            try
            {
                return (await BASE_API_URL.PostToUrlAsync(args)).FromJson<PushResponse>();
            }
            catch (WebException webEx)
            {
                return webEx.GetResponseBody().FromJson<PushResponse>();
            }
        }



        private object CreateArgs(string title, string message, string userKey, string device, Priority priority, DateTime timestamp, NotificationSound notificationSound)
        {
            // Try the passed user key or fall back to default
            var userGroupKey = string.IsNullOrEmpty(userKey) ? DefaultUserGroupSendKey : userKey;

            if (string.IsNullOrEmpty(userGroupKey))
            {
                throw new ArgumentException("User key must be supplied", nameof(userKey));
            }

            var args = new PushoverRequestArguments()
            {
                token = AppKey,
                user = userGroupKey,
                device = device,
                title = title,
                message = message,
                timestamp = (int)timestamp.Subtract(new DateTime(1970, 1, 1)).TotalSeconds,
                priority = (int)priority
            };

            if (notificationSound != NotificationSound.NotSet)
            {
                args.sound = notificationSound.ToString().ToLower();
            }

            return args;
        }


    }
}
