using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using ViewModel;
using DataAccess;
using InterFaces;
using System.Data;
using System.Data.OleDb;

namespace UnitTest
{
    [TestClass]
    public class PersonRepositoryTest
    {
        private IDbConnection connection;

        public TestContext TestContext { get; set; }

        [ClassInitialize]
        public static void CleanUpDb(TestContext context)
        {
            const string provider = "Microsoft.ACE.OLEDB.12.0";
            const string dataSourceUri = @"PersonInfo.accdb";
            const string password = "yhyahaha";

            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Provider = provider;
            builder.DataSource = dataSourceUri;
            builder["Jet OLEDB:Database Password"] = password;

            using (OleDbConnection connection = new OleDbConnection(builder.ConnectionString))
            using (OleDbCommand command = new OleDbCommand())
            {
                command.Connection = connection;
                command.CommandText = "DELETE FROM Person WHERE ID >= 4 ";

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        [TestInitialize]
        public void TestInitializer()
        {
            const string provider = "Microsoft.ACE.OLEDB.12.0";
            const string dataSourceUri = @"PersonInfo.accdb";
            const string password = "yhyahaha";

            OleDbConnectionStringBuilder builder = new OleDbConnectionStringBuilder();
            builder.Provider = provider;
            builder.DataSource = dataSourceUri;
            builder["Jet OLEDB:Database Password"] = password;

            connection = new OleDbConnection(builder.ConnectionString);
        }

        [TestCleanup]
        public void CleanUp()
        {
            connection.Dispose();
        }

        [TestMethod]
        public void GetNextID()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);

            // Act
            var pbObj = new PrivateObject(repository);
            int ret = (int)pbObj.Invoke("GetNextId", null);

            // Assert
            Console.WriteLine(ret);
        }

        [TestMethod]
        public void GetItemById()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);

            // Act
            Person person = repository.GetItemById(1);

            // Assert
            Assert.AreEqual("Ichiro", person.PersonName);
        }

        [TestMethod]
        public void GetItemByIdItDoseNotExist()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);

            // Act
            Person person = repository.GetItemById(100);

            // Assert
            Assert.AreEqual(0, person.Id);
        }

        [TestMethod]
        public void GetItemByIdItIsLessThan1()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);

            // Act
            Person person = repository.GetItemById(0);

            // Assert
            Assert.AreEqual(0, person.Id);
        }

        [TestMethod]
        public void GetItemsByKeyword()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);
            string keyword = "Ichi";
            IList<Person> items;

            // Act
            items = repository.GetItemsByKeyword(keyword);

            // Assert
            Assert.AreEqual(1, items.Count);
        }

        [TestMethod]
        public void AddItem()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);
            var pbObj = new PrivateObject(repository);

            int newId = (int)pbObj.Invoke("GetNextId", null);

            // Act
            Person person = new Person { Id = 0, PersonName = "Goro", Age = 3 };
            int res = repository.AddItem(person);

            // Assert
            Assert.AreEqual(0, newId - res);
            Console.WriteLine("NewId:" + newId + " Res:" + res);
        }

        [TestMethod]
        public void UpdateItem()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);

            Person person = new Person { PersonName = "hachi", Age = 32 };
            repository.AddItem(person);

            Person hachi = repository.GetItemsByKeyword("hachi").FirstOrDefault();
            Assert.AreEqual(32, hachi.Age);

            // Act
            hachi.Age = 28;
            repository.UpdateItem(hachi);

            // Assert
            var res = repository.GetItemsByKeyword("hachi").FirstOrDefault();
            Assert.AreEqual(28, res.Age);
        }

        [TestMethod]
        public void DeleteItem()
        {
            // Arrange
            PersonRepository repository = new PersonRepository(connection);
            Person person = new Person { Id = 0, PersonName = "chiko", Age = 5 };
            repository.AddItem(person);

            var chiko = repository.GetItemsByKeyword(person.PersonName).First();

            // Act
            int res = repository.DeleteItem(chiko);

            // Assert
            Assert.AreEqual(1, res);
        }
    }
}
