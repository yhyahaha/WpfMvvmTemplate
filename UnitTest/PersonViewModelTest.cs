using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using ViewModel;

namespace UnitTest
{   
    [TestClass]
    public class PersonViewModelTest
    {
        [TestMethod]
        public void ConstructorWithNoArges()
        {
            // Arrange

            // Act
            PersonViewModel pv = new PersonViewModel();

            // Assert
            Assert.AreEqual(0, pv.Id);
            Assert.AreEqual(string.Empty, pv.PersonName);
            Assert.AreEqual(null, pv.Age);
        }

        [TestMethod]
        public void ConstructorWithArges()
        {
            // Arrange
            Person p = new Person { Id = 1, PersonName = "Taro", Age = 5 };

            // Act
            PersonViewModel sut = new PersonViewModel(p);

            // Assert
            Assert.AreEqual(1, sut.Id);
            Assert.AreEqual("Taro", sut.PersonName);
            Assert.AreEqual(5, sut.Age);
        }

        [TestMethod]
        public void ErrorInfoTestAgeValidation()
        {
            // Arrange
            Person p = new Person();
            PersonViewModel sut = new PersonViewModel(p);

            // Act/Assert

            // Collect
            sut.Age = 0;
            Assert.IsFalse(sut.HasErrors);

            sut.Age = 100;
            Assert.IsFalse(sut.HasErrors);

            sut.Age = null; // null は許容
            Assert.IsFalse(sut.HasErrors);

            // Wrong
            sut.Age = -1;
            Assert.IsTrue(sut.HasErrors);

            sut.Age = 121;
            Assert.IsTrue(sut.HasErrors);
        }

        [TestMethod]
        public void ErrorInfoPersonNameValidation()
        {
            // Arrange
            Person p = new Person();
            PersonViewModel sut = new PersonViewModel(p);

            // Act/Assert

            // Collect
            sut.PersonName = "Taro";
            Assert.IsFalse(sut.HasErrors);

            sut.PersonName = string.Empty; // [Required(AllowEmptyStrings = true)]
            Assert.IsFalse(sut.HasErrors);

            // Wrong
            sut.PersonName = "ElevenLengt";
            Assert.IsTrue(sut.HasErrors);

            sut.PersonName = null;
            Assert.IsTrue(sut.HasErrors);
        }

    }
}
