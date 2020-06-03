using InterFaces;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace DataAccess
{
    public sealed class PersonRepository : IDataAccess<Person> 
    {
        // Fields
        private IDbConnection connection;
        private IList<Person> people;

        // Constructor
        public PersonRepository(IDbConnection connection)
        {
            this.connection = connection;
        }

        // IDataAccess Members

        /// <summary>
        /// DBの全てのデータを返す
        /// </summary>
        /// <returns>IList</returns>
        public IList<Person> GetAllItems()
        {
            people = new List<Person>();
            
            // Create IDbCommand object.
            IDbCommand command = connection.CreateCommand();

            // Start a local transaction
            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            // Assign both transaction object and connection
            // to Command object for a pending local transaction.
            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT * FROM Person";

                var dataReader = command.ExecuteReader();
                while (dataReader.Read() == true)
                {
                    int res1 = dataReader.GetInt32(0);
                    string res2 = dataReader.GetString(1);
                    int res3 = dataReader.GetInt32(2);

                    Person p = new Person(res1, res2, res3);
                    people.Add(p);
                }
                dataReader.Close();

                transaction.Commit();
            }
            catch(Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return people;
        }

        /// <summary>
        /// キーワードを氏名に含むitemを返す
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns>IList</returns>
        public IList<Person> GetItemsByKeyword(string keyword)
        {
            people = new List<Person>();

            IDbCommand command = connection.CreateCommand();

            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;

                var param = command.CreateParameter();
                param.ParameterName = "@keyword";
                param.Value = '%' + keyword + '%';
                command.Parameters.Add(param);

                command.CommandText = "SELECT * FROM Person WHERE PersonName Like @keyword ";

                var dataReader = command.ExecuteReader();
                while (dataReader.Read() == true)
                {
                    int res1 = dataReader.GetInt32(0);
                    string res2 = dataReader.GetString(1);
                    int res3 = dataReader.GetInt32(2);

                    Person p = new Person(res1, res2, res3);
                    people.Add(p);
                }
                dataReader.Close();

                command.Transaction.Commit();
            }
            catch(Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return people;
        }

        /// <summary>
        /// Id でitemを検索する。
        /// </summary>
        /// <param name="id">1以上</param>
        /// <returns>Person</returns>
        public Person GetItemById(int id)
        {
            Person person = new Person();

            if (id < 1) return person;

            IDbCommand command = connection.CreateCommand();

            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;

                var param = command.CreateParameter();
                param.ParameterName = "@Id";
                param.Value = id;
                command.Parameters.Add(param);
                        
                command.CommandText = "SELECT * FROM Person WHERE Id=@id ";

                var dataReader = command.ExecuteReader();
                while (dataReader.Read() == true)
                {
                    int res1 = dataReader.GetInt32(0);
                    string res2 = dataReader.GetString(1);
                    int res3 = dataReader.GetInt32(2);

                    person = new Person(res1, res2, res3);
                }
                dataReader.Close();

                command.Transaction.Commit();
            }
            catch(Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return person;
        }

        /// <summary>
        /// Itemを追加する
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Idを返す。エラー時は0</returns>
        public int AddItem(Person item)
        {
            if (item == null) return 0;

            int newId = GetNextId();

            IDbCommand command = connection.CreateCommand();

            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;

                var param1 = command.CreateParameter();
                param1.ParameterName = "@id";
                param1.Value = newId;
                command.Parameters.Add(param1);

                var param2 = command.CreateParameter();
                param2.ParameterName = "@name";
                param2.Value = item?.PersonName;
                command.Parameters.Add(param2);

                var param3 = command.CreateParameter();
                param3.ParameterName = "@age";
                param3.Value = item.Age;
                command.Parameters.Add(param3);
         
                command.CommandText = "INSERT INTO Person (ID, PersonName, Age) " +
                                        "VALUES (@id, @name, @age) ";

                command.ExecuteNonQuery();

                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                newId = 0;

                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return newId;
        }

        /// <summary>
        /// itemを更新する
        /// </summary>
        /// <param name="item"></param>
        /// <returns>成功1,失敗0</returns>
        public int UpdateItem(Person item)
        {
            int res = 0;

            if (item == null) return res;
            if (item.Id == 0) return res;

            IDbCommand command = connection.CreateCommand();

            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;

                var param1 = command.CreateParameter();
                param1.ParameterName = "@name";
                param1.Value = item?.PersonName;
                command.Parameters.Add(param1);

                var param2 = command.CreateParameter();
                param2.ParameterName = "@age";
                param2.Value = item.Age;
                command.Parameters.Add(param2);

                var param3 = command.CreateParameter();
                param3.ParameterName = "@id";
                param3.Value = item.Id;
                command.Parameters.Add(param3);

                command.CommandText = "UPDATE Person " + 
                                        "SET PersonName = @name, Age = @age " +
                                        "WHERE ID = @id ";

                res = command.ExecuteNonQuery();

                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return res;
        }

        /// <summary>
        /// itemを削除
        /// </summary>
        /// <param name="item"></param>
        /// <returns>成功1,失敗0</returns>
        public int DeleteItem(Person item)
        {
            int res = 0;
            if (item == null) return res;
            if (item.Id == 0) return res;

            IDbCommand command = connection.CreateCommand();

            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;

                var param1 = command.CreateParameter();
                param1.ParameterName = "@id";
                param1.Value = item.Id;
                command.Parameters.Add(param1);

                command.CommandText = "DELETE FROM Person WHERE ID=@id ";

                res = command.ExecuteNonQuery();

                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return res;
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        // Private Helpers
        private int GetNextId()
        {
            int id = 0;
            
            IDbCommand command = connection.CreateCommand();

            connection.Open();
            IDbTransaction transaction = connection.BeginTransaction();

            command.Connection = connection;
            command.Transaction = transaction;

            try
            {
                command.CommandType = CommandType.Text;
                command.CommandText = "SELECT MAX(ID) AS MAXID FROM Person ";

                id = Convert.ToInt32(command.ExecuteScalar(),CultureInfo.CurrentCulture) + 1;
                
                command.Transaction.Commit();
            }
            catch (Exception ex)
            {
                try
                {
                    transaction.Rollback();
                }
                catch (Exception)
                {
                    throw new Exception(ex.Message);
                }
            }
            finally
            {
                command.Dispose();
                transaction.Dispose();
                connection.Close();
            }

            return id;
        }
    }
}
