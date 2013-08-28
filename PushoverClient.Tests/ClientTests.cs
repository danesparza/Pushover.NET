using System;
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
            string testAppKey = "a4RoPkZ7GYNU1XiiHHybqQG9EWMuTt";
            string testUserKey = "uC3wSuMNinuxKWsia6f6yox62vb13w";
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
