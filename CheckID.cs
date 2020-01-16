using System.Collections.Generic;
using System.Linq;

namespace WebServiceAndDatabaseExample
{
   /* public class CheckID
    {
        private string ConnectionString { get; }

        public List<TestLogin> Login { get; }

        public CheckID(string connectionString)
        {
            ConnectionString = connectionString;

            using var connection = ConnectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from TestLogin";
            Login = command.GetDataTable().Select().Select(x =>
                new TestLogin((string)x["LoginID"], (int)x["CustomerID"], (string)x["PasswordHash"])).ToList();
        }

        public void InsertLogin(TestLogin login)
        {
            using var connection = ConnectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into Person (LoginID,CustomerID,PasswordHash) values (@LoginID, @CustomerID, @PasswordHash)";
            command.Parameters.AddWithValue("personID", login.LoginID);
            command.Parameters.AddWithValue("CustomerID", login.CustomerID);
            command.Parameters.AddWithValue("PasswordHash", login.PasswordHash);

            command.ExecuteNonQuery();
        }

    }*/
}
