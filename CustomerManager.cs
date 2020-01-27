using System.Collections.Generic;
using System.Linq;

namespace WebServiceAndDatabaseExample
{
    public class CustomerManager
    {
        private string ConnectionString { get; }

        public List<Customer> custo { get; }

        public CustomerManager(string connectionString)
        {
            ConnectionString = connectionString;
            Menu m = new Menu();
            using var connection = ConnectionString.CreateConnection();
            var command = connection.CreateCommand();
            command.CommandText = "select * from TestCustomer ";
            //command.CommandText = "select * from TestCustomer ";
            custo = command.GetDataTable().Select().Select(x =>
                new Customer((int)x["CustomerID"], (string)x["Name"], (string)x["Address"], (string)x["City"], (string)x["PostCode"])).ToList();
        }
        
        public void InsertPerson(Customer cus)
        {
            using var connection = ConnectionString.CreateConnection();
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                "insert into TestCustomer (CustomerID,Name,Address,City,PostCode)values (@CustomerID, @Name, @Address,@City,@PostCode)";
            command.Parameters.AddWithValue("CustomerID", cus.CustomerID);
            command.Parameters.AddWithValue("Name",cus.Name );
            command.Parameters.AddWithValue("Address", cus.Address);
            command.Parameters.AddWithValue("City", cus.City);
            command.Parameters.AddWithValue("PostCode", cus.PostCode);

            command.ExecuteNonQuery();
        }
        
    }
}
