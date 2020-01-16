using System.Collections.Generic;
using System.Linq;

namespace WebServiceAndDatabaseExample
{
    class TransactionManager
    {
        private string ConnectionString { get; }

        public List<Transaction> Transac { get; }

        public TransactionManager(string connectionString)
        {
            ConnectionString = connectionString;

            using var connection = ConnectionString.CreateConnection();
            var command = connection.CreateCommand();
            //int CustoID = CustoID.getCustomerID();
            command.CommandText = "SELECT* FROM TestTransaction";
            Transac = command.GetDataTable().Select().Select(x =>
                new Transaction((int)x["TransactionID"], (string)x["TransactionType"], (int)x["AccountNumber"], (int)x["DestinationAccountNumber"], (decimal)x["Amount"], (string)x["Comment"], (string)x["TransactionTimeUtc"])).ToList();
        }

        public void InsertPerson(Transaction transaction)
        {
            using var connection = ConnectionString.CreateConnection();
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText =
                "insert into TestTransaction (TransactionID,TransactionType,AccountNumber,DestinationAccountNumber,Amount,Comment,TransactionTimeUtc) values (@TransactionID,@TransactionType,@AccountNumber,@DestinationAccountNumber,@Amount,@Comment,@TransactionTimeUtc)";
            command.Parameters.AddWithValue("TransactionID",transaction.TransactionID);
            command.Parameters.AddWithValue("TransactionType", transaction.TransactionType);
            command.Parameters.AddWithValue("AccountNumber",transaction.AccountNumber);
            command.Parameters.AddWithValue("DestinationAccountNumber",transaction.DestinationAccountNumber);
            command.Parameters.AddWithValue("Amount",transaction.Amount);
            command.Parameters.AddWithValue("Comment",transaction.Comment);
            command.Parameters.AddWithValue("TransactionTimeUtc",transaction.TransactionTimeUtc);
            command.ExecuteNonQuery();
        }
    }
}
