using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebServiceAndDatabaseExample
{
    class PayeeID
    {
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        private static string ConnectionString { get; } = Configuration["ConnectionString"];
        public int ID;
        string connStr = ConnectionString;
        SqlConnection conn = null;
        SqlDataReader dr = null;
        public PayeeID(int ID) 
        {
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into TestCustomer( CustomerID ) values" + "(" + "'" + ID + "'" + ")";
                SqlDataReader reader = cmd.ExecuteReader();

            }
            catch (Exception e)
            { Console.WriteLine(e); }
        }
    }
}
