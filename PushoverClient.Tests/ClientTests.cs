using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PushoverClient.Tests
{
    [TestClass]
    public class ClientTests
    {
        [TestMethod]
        public void PushWithValidParms_ReturnsSuccessful()
        {
            //  Arrange
            string testAppKey = "YOURAPPKEY";
            string testUserKey = "YOURUSERKEY";
            string title = "Test title";
            string message = "This is a test push notification message";

            //  Act
            Pushover pclient = new Pushover(testAppKey);
            PushResponse response = pclient.Push(title, message, testUserKey);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual<int>(1, response.Status);
        }
    }
}
