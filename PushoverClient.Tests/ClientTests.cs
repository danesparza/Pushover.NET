using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;

namespace PushoverClient.Tests
{
    [TestClass]
    public class ClientTests
    {
        private const string TEST_APP_KEY = "YOURAPPKEY";
        private const string TEST_USER_KEY = "YOURUSERKEY";

        [TestMethod]
        public void PushWithValidParms_ReturnsSuccessful()
        {
            //  Arrange
            var title = "Test title";
            var message = "This is a test push notification message";

            //  Act
            var pclient = new Pushover(TEST_APP_KEY);
            var response = pclient.Push(title, message, TEST_USER_KEY);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }

        [TestMethod]
        public async Task PushAsyncWithValidParms_ReturnsSuccessful()
        {
            //  Arrange
            var title = "Test title";
            var message = "This is a test push notification message";

            //  Act
            var pclient = new Pushover(TEST_APP_KEY);
            var response = await pclient.PushAsync(title, message, TEST_USER_KEY);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task PushWithNoKey_ReturnsError()
        {
            //  Arrange
            var title = "Test title";
            var message = "This is a test push notification message";

            //  Act
            var pclient = new Pushover(TEST_APP_KEY);
            var response = await pclient.PushAsync(title, message);

            //  Assert - above code should error before this
            Assert.Fail();
        }

        [TestMethod]
        public async Task PushWithDefaultKey_ReturnsSuccessful()
        {
            //  Arrange
            var title = "Test title";
            var message = "This is a test push notification message";

            //  Act
            var pclient = new Pushover(TEST_APP_KEY) { DefaultUserGroupSendKey = TEST_USER_KEY };
            var response = await pclient.PushAsync(title, message);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }

        [TestMethod]
        public async Task PushWithHighPriority_ReturnsSuccessful()
        {
            //  Arrange
            var title = "Test title";
            var message = "This is a test push notification message";
            var priority = Priority.High;

            //  Act
            var pclient = new Pushover(TEST_APP_KEY) { DefaultUserGroupSendKey = TEST_USER_KEY };
            var response = await pclient.PushAsync(title, message, priority: priority);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }

        [TestMethod]
        public async Task PushWithSound_ReturnsSuccessful()
        {
            //  Arrange
            var title = "Test title";
            var message = "This is a test push notification message";

            //  Act
            var pclient = new Pushover(TEST_APP_KEY) { DefaultUserGroupSendKey = TEST_USER_KEY };
            var response = await pclient.PushAsync(title, message, notificationSound: NotificationSound.Alien);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }
    }
}
