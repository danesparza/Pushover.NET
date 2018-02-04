using System;
using ServiceStack;
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
            this.AppKey = appKey;
        }

        /// <summary>
        /// Create a pushover client using both a source 
        /// application key and a default send key
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="defaultSendKey"></param>
        public Pushover(string appKey, string defaultSendKey) : this(appKey)
        {
            this.DefaultUserGroupSendKey = defaultSendKey;   
        }

        /// <summary>
        /// Push a message
        /// </summary>
        /// <param name="title">Message title</param>
        /// <param name="message">The body of the message</param>
        /// <param name="userKey">The user or group key (optional if you have set a default already)</param>
        /// <param name="device">Send to a specific device</param>
        /// <returns></returns>
        public PushResponse Push(string title, string message, string userKey = "", string device = "")
        {
            var args = CreateArgs(title, message, userKey, device);
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
        /// <returns></returns>
        public async Task<PushResponse> PushAsync(string title, string message, string userKey = "", string device = "")
        {
            var args = CreateArgs(title, message, userKey, device);
            try
            {
                return (await BASE_API_URL.PostToUrlAsync(args)).FromJson<PushResponse>();
            }
            catch (WebException webEx)
            {
                return webEx.GetResponseBody().FromJson<PushResponse>();
            }
        }

        private object CreateArgs(string title, string message, string userKey, string device)
        {
            // Try the passed user key or fall back to default
            string userGroupKey = string.IsNullOrEmpty(userKey) ? this.DefaultUserGroupSendKey : userKey;

            if (string.IsNullOrEmpty(userGroupKey))
                throw new ArgumentException("User key must be supplied", nameof(userKey));

            return new
            {
                token = this.AppKey,
                user = userGroupKey,
                device,
                title,
                message
            };
        }
    }
}
