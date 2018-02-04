using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            string title = "Test title";
            string message = "This is a test push notification message";

            //  Act
            Pushover pclient = new Pushover(TEST_APP_KEY);
            PushResponse response = pclient.Push(title, message, TEST_USER_KEY);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }

        [TestMethod]
        public async Task PushAsyncWithValidParms_ReturnsSuccessful()
        {
            //  Arrange
            string title = "Test title";
            string message = "This is a test push notification message";

            //  Act
            Pushover pclient = new Pushover(TEST_APP_KEY);
            PushResponse response = await pclient.PushAsync(title, message, TEST_USER_KEY);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task PushWithNoKey_ReturnsError()
        {
            //  Arrange
            string title = "Test title";
            string message = "This is a test push notification message";

            //  Act
            Pushover pclient = new Pushover(TEST_APP_KEY);
            PushResponse response = await pclient.PushAsync(title, message);

            //  Assert - above code should error before this
            Assert.Fail();
        }

        [TestMethod]
        public async Task PushWithDefaultKey_ReturnsSuccessful()
        {
            //  Arrange
            string title = "Test title";
            string message = "This is a test push notification message";

            //  Act
            Pushover pclient = new Pushover(TEST_APP_KEY) { DefaultUserGroupSendKey = TEST_USER_KEY };
            PushResponse response = await pclient.PushAsync(title, message);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(1, response.Status);
        }
    }
}
