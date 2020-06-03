using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using ViewModel;
using InterFaces;
using Moq;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace UnitTest
{
    [TestClass]
    public class MainWindowViewModelTest
    {
        private IList<Person> items;
        private ObservableCollection<PersonViewModel> people;
        private Mock<IDataAccess<Person>> mockRepository;
        private Mock<IObjectMapping> mockMapper;
        private Mock<IObjectMapping> mockReverseMapper;

        [TestInitialize]
        public void TestInitializer()
        {
            Person p1 = new Person { Id = 1, PersonName = "Taro", Age = 25 };
            Person p2 = new Person { Id = 2, PersonName = "Jiro", Age = 20 };
            Person p3 = new Person { Id = 3, PersonName = "Satsuki", Age = 5 };
            items = new List<Person>();
            items.Add(p1); items.Add(p2); items.Add(p3);

            mockRepository = new Mock<IDataAccess<Person>>();
            mockRepository.Setup(x => x.GetAllItems()).Returns(items);

            PersonViewModel pv1 = new PersonViewModel(p1);
            PersonViewModel pv2 = new PersonViewModel(p2);
            PersonViewModel pv3 = new PersonViewModel(p3);
            people = new ObservableCollection<PersonViewModel>();
            people.Add(pv1); people.Add(pv2); people.Add(pv3);

            mockMapper = new Mock<IObjectMapping>();
            mockMapper.Setup(x => x.Map<IList<Person>, ObservableCollection<PersonViewModel>>(items))
                .Returns(people);

            mockReverseMapper = new Mock<IObjectMapping>();
        }

        [TestCleanup]
        public void TestClearner()
        {
            items = null;
            people = null;
            mockRepository = null;
            mockMapper = null;
        }


        [TestMethod]
        public void TestRestMethod()
        {
            // Arrange
            var sut = new MainWindowViewModel(mockRepository.Object, mockMapper.Object, mockReverseMapper.Object);

            // ACT
            var pbObj = new PrivateObject(sut);
            var ret = pbObj.Invoke("Reset");

            // Assert
            Assert.AreEqual(3, sut.People.Count);
            Assert.AreEqual(0, sut.Person.Id);
            Assert.AreEqual(-1, sut.SelectedIndex);
        }

        [TestMethod]
        public void TestAddNewItemCommand()
        {
            // Arrange
            var sut = new MainWindowViewModel(mockRepository.Object, mockMapper.Object, mockReverseMapper.Object);
            sut.SelectedIndex = 1;
            sut.Person = null;

            // ACT
            ICommand command = sut.ClearItemCommand;
            command.Execute(null);

            // Assert
            Assert.AreEqual(0, sut.Person.Id);
            Assert.AreEqual(-1, sut.SelectedIndex);

            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        public void UpdateCommandUpdateNewItem()
        {
            // Arrange
            var sut = new MainWindowViewModel(mockRepository.Object, mockMapper.Object, mockReverseMapper.Object);
            sut.SelectedIndex = -1;
            Person p = new Person(0, "Sanjuro", 30);
            PersonViewModel person = new PersonViewModel(p);
            sut.Person = person;

            mockRepository.Setup(x => x.AddItem(p)).Returns(4).Verifiable();
            mockReverseMapper.Setup(x => x.Map<PersonViewModel, Model.Person>(person)).Returns(p);

            // ACT
            ICommand command = sut.UpdateItemCommand;
            command.Execute(null);

            // Assert
            mockRepository.Verify();
            Assert.AreEqual(-1, sut.SelectedIndex);
            Assert.AreEqual(4, sut.People.Count);
        }

        [TestMethod]
        public void UpdateCommandUpdateExistingItem()
        {
            // Arrange
            var sut = new MainWindowViewModel(mockRepository.Object, mockMapper.Object, mockReverseMapper.Object);
            sut.SelectedIndex = 2;
            Assert.AreEqual("Satsuki", sut.Person.PersonName);

            string newName = "Sacchan";
            sut.Person.PersonName = newName;
            Model.Person person = new Person { Id = 3, PersonName = newName, Age = 5 };
            mockReverseMapper.Setup(x => x.Map<PersonViewModel, Model.Person>(sut.Person)).Returns(person).Verifiable();

            PersonViewModel pv = new PersonViewModel(person);
            mockMapper.Setup(x => x.Map<Model.Person, PersonViewModel>(person)).Returns(pv).Verifiable();

            mockRepository.Setup(x => x.UpdateItem(person)).Returns(1).Verifiable();

            // Act
            ICommand command = sut.UpdateItemCommand;
            command.Execute(null);

            // Assert
            mockRepository.Verify();
            mockMapper.Verify();
            mockRepository.Verify();
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Arrange
            var sut = new MainWindowViewModel(mockRepository.Object, mockMapper.Object, mockReverseMapper.Object);
            sut.SelectedIndex = 1;
            Assert.AreEqual("Jiro", sut.Person.PersonName);
            Assert.AreEqual(3, sut.People.Count);

            Model.Person p = new Person { Id = 2, PersonName = "Jiro", Age = 20 };
            mockReverseMapper.Setup(x => x.Map<PersonViewModel, Model.Person>(sut.Person)).Returns(p).Verifiable();

            mockRepository.Setup(x => x.DeleteItem(p)).Returns(1).Verifiable();

            // Act
            ICommand command = sut.DeleteItemCommand;
            command.Execute(null);

            // Assert
            Assert.AreEqual(2, sut.People.Count);
            mockReverseMapper.Verify();
            mockRepository.Verify();
        }

        [TestMethod]
        public void SerchItem()
        {
            // Arrange
            var sut = new MainWindowViewModel(mockRepository.Object, mockMapper.Object, mockReverseMapper.Object);
            sut.Keyword = "sat";

            Person p3 = new Person { Id = 3, PersonName = "Satsuki", Age = 5 };
            var res = new List<Person>();
            res.Add(p3);

            mockRepository.Setup(x => x.GetItemsByKeyword("sat")).Returns(res);

            PersonViewModel pv3 = new PersonViewModel(p3);
            ObservableCollection<PersonViewModel> obs = new ObservableCollection<PersonViewModel>();
            obs.Add(pv3);
            mockMapper.Setup(x => x.Map<IList<Model.Person>, ObservableCollection<PersonViewModel>>(res)).Returns(obs);

            // Act
            ICommand command = sut.SearchCommand;
            command.Execute(null);

            // Assert
            Assert.AreEqual(1, sut.People.Count);
            Assert.AreEqual("Satsuki", sut.Person.PersonName);
        }
    }
}
