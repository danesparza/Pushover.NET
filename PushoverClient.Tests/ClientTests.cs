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
            var testAppKey = "";
            var testUserKey = "";
            var title = "Test title";
            var message = "This is a test push notification message";

            //  Act
            var pclient = new Pushover(testAppKey);
            var  response = pclient.Push(title, message, testUserKey);

            //  Assert
            Assert.IsNotNull(response);
            Assert.AreEqual<int>(1, response.Status);
        }
    }
}
