using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System.ComponentModel.DataAnnotations;

namespace UnitTest
{
    [TestClass]
    public class PersonTest
    {
        [TestMethod]
        public void ConstructorWithoutArges()
        {
            // Arrange

            // Act
            Person p = new Person();

            // Assert
            Assert.AreEqual(0, p.Id);
            Assert.AreEqual(string.Empty, p.PersonName);
            Assert.AreEqual(null, p.Age);
        }

        [TestMethod]
        public void ConstructorWithCollectArges()
        {
            // Arrange

            // Act
            Person p = new Person(1,"Taro",15);

            // Assert
            Assert.AreEqual(1, p.Id);
            Assert.AreEqual("Taro", p.PersonName);
            Assert.AreEqual(15, p.Age);
        }
    }
}
