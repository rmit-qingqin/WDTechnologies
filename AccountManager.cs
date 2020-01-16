using System.Collections.Generic;
using System.Linq;

namespace WebServiceAndDatabaseExample
{
    public class AccountManager
    {
        private string ConnectionString { get; }

        public List<Account> acc { get; }

        public AccountManager(string connectionString)
        {
            ConnectionString = connectionString;
            using var connection = ConnectionString.CreateConnection();
            var command = connection.CreateCommand();
            //int CustoID = CustoID.getCustomerID();
            command.CommandText = "select * from TestAccount";
            acc = command.GetDataTable().Select().Select(x =>
                new Account((int)x["AccountNumber"], (string)x["AccountType"], (int)x["CustomerID"], (decimal)x["Balance"])).ToList();
        }

        public void InsertPerson(Account account)
        {
            using var connection = ConnectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into TestAccount (AccountNumber, AccountType, CustomerID, Balance) values (@accountNumber, @accountType, @customerID, @balance)";
            command.Parameters.AddWithValue("AccountNumber", account.AccountNumber);
            command.Parameters.AddWithValue("CustomerID", account.AccountType);
            command.Parameters.AddWithValue("PasswordHash", account.CustomerID);
            command.Parameters.AddWithValue("PasswordHash", account.Balance);
            command.ExecuteNonQuery();
        }
       
    }
}

