using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
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
        /// The shared http client
        /// </summary>
        private readonly HttpClient Client = new HttpClient();
        
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
        /// The interval (in seconds) between repeating notifications for
        /// <see cref="Priority.Emergency"/> priority messages. Pushover
        /// servers keep sending emergency priority messages until they
        /// are either acknowledged or the acknowledgement time expires
        /// (<see cref="EmergencyExpiryInterval"/>).
        /// </summary>
        public int EmergencyRetryInterval { get; set; } = 60; // Seconds

        /// <summary>
        /// The interval (in seconds) before <see cref="Priority.Emergency"/>
        /// messages expire. 
        /// </summary>
        public int EmergencyExpiryInterval { get; set; } = 300; // Seconds;

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
        /// <param name="attachment">If set sends a file attachment</param>
        /// <returns></returns>
        public PushResponse Push(string title, string message, string userKey = "", string device = "", Priority priority = Priority.Normal, DateTime? timestamp = null, NotificationSound notificationSound = NotificationSound.NotSet, PushoverFileAttachment attachment = null)
        {
            ValidateAttachment(attachment);
            var task = PostAsync(CreateArgs(title, message, userKey, device, priority, timestamp ?? DateTime.UtcNow, notificationSound), attachment);
            task.Wait();
            return task.Result;
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
        /// <param name="attachment">If set sends a file attachment</param>
        /// <returns></returns>
        public async Task<PushResponse> PushAsync(string title, string message, string userKey = "", string device = "", Priority priority = Priority.Normal, DateTime? timestamp = null, NotificationSound notificationSound = NotificationSound.NotSet, PushoverFileAttachment attachment = null)
        {
            ValidateAttachment(attachment);
            return await PostAsync(CreateArgs(title, message, userKey, device, priority, timestamp ?? DateTime.UtcNow, notificationSound), attachment);          
        }

        private PushoverRequestArguments CreateArgs(string title, string message, string userKey, string device, Priority priority, DateTime timestamp, NotificationSound notificationSound)
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

            if (priority == Priority.Emergency)
            {
                args.retry = EmergencyRetryInterval;
                args.expire = EmergencyExpiryInterval;
            }

            return args;
        }

        private void ValidateAttachment(PushoverFileAttachment attachment)
        {
            // No attachment is fine.
            if(attachment == null)
            {
                return;
            }
            attachment.Validate();
        }

        private async Task<PushResponse> PostAsync(PushoverRequestArguments args, PushoverFileAttachment attachment)
        {
            StreamContent fileContent = null;
            try
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Add the required fields (or their defaults)
                    content.Add(new StringContent(args.token), "token");
                    content.Add(new StringContent(args.user), "user");
                    content.Add(new StringContent(args.device), "device");
                    content.Add(new StringContent(args.title), "title");
                    content.Add(new StringContent(args.message), "message");
                    content.Add(new StringContent(args.timestamp.ToString()), "timestamp");
                    content.Add(new StringContent(args.priority.ToString()), "priority");

                    // Add the attachment if there is one.
                    if (attachment != null)
                    {
                        fileContent = new StreamContent(attachment.FileStream);
                        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(attachment.ContentType);
                        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                        {
                            Name = "attachment",
                            FileName = attachment.FileName
                        };
                        content.Add(fileContent);
                    }

                    // Make the request.
                    try
                    {
                        var result = await Client.PostAsync(BASE_API_URL, content);
                        // Always try to read the body as the expected json result, even if the status code is not OK.
                        return await ReadStreamAsync(await result.Content.ReadAsStreamAsync());
                    }
                    catch (WebException webEx)
                    {
                        return await ReadStreamAsync(webEx.Response.GetResponseStream());
                    }
                }
            }
            finally
            {
                // Dispose of the file content, if there is one.
                if(fileContent != null)
                {
                    fileContent.Dispose();
                }
            }
        }

        private async Task<PushResponse> ReadStreamAsync(Stream s)
        {
            using (var sr = new StreamReader(s))
            {
                return JsonConvert.DeserializeObject<PushResponse>(await sr.ReadToEndAsync());
            }
        }
    }
}
