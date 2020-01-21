using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace WebServiceAndDatabaseExample
{
    class BankAccount
    {
        private decimal balance;
        public decimal amount;
        private CustomerManager CustomerManager { get; }
        private TransactionManager TransactionManager { get; }
        private AccountManager AccountManager { get;  }
        public BankAccount(string connectionString)
        {
            AccountManager = new AccountManager(connectionString);
            TransactionManager = new TransactionManager(connectionString);
            CustomerManager = new CustomerManager(connectionString);
        }
        string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
        SqlConnection conn = null;
        SqlDataReader dr = null;

        public BankAccount()
        { }
       
        public void UserActionMune(int ID)
        {        
            while (true)
            {
                Console.WriteLine("Please choose your function: ");
                Console.Write("1.Deposit 2.Withdraw 3.Transfer 4.DisplayTransaction 5.Customer 6.Log out 7.Exit ");
                Console.WriteLine();
                var input = Console.ReadLine();
                Console.WriteLine();
                if (!int.TryParse(input, out var option) || !option.IsInRange(1, 7))
                {
                    Console.WriteLine("Invalid input.");
                    Console.WriteLine();
                    continue;
                }
                switch (option)
                {
                    case 1:
                        Deposit(ID);
                        break;
                    case 2:
                        Withdraw(ID);
                        break;
                    case 3:
                        Transfer(ID);
                        break;
                    case 4:
                        DisplayTransaction(ID);
                        break;
                    case 5:
                        DisplayCustomer(ID);
                        break;
                    case 6:
                        Menu meu = new Menu();
                        meu.Run();
                        break;
                    case 7:
                        System.Environment.Exit(0);
                        break;                    
                    default:
                        throw new Exception();
                }
            }
        }
        public void Deposit(int ID)
        {
            Console.WriteLine("Please enter the deposit amount：");
            amount = decimal.Parse(Console.ReadLine());
            try
            {
                conn = new SqlConnection(connStr);
                conn.Close();
                conn.Open();
                string sql = "UPDATE TestAccount SET Balance = Balance + " + amount + " WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";                
                SqlCommand cmd = new SqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            conn.Close();
            UserActionMune(ID);        

        }

        public bool Withdraw(int ID)
        {
            Console.WriteLine("Please enter the withdrawal amount：");
            amount = decimal.Parse(Console.ReadLine());    
            if (Check(ID,amount)==false)
            {
                Console.WriteLine("Insufficient balance！");
                UserActionMune(ID);
                return false;
            }            
            try
            {
                conn = new SqlConnection(connStr);
                conn.Close();
                conn.Open();
                string sql = "UPDATE TestAccount SET Balance = Balance - " + amount + " WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            try
            {
                conn = new SqlConnection(connStr);
                conn.Close();
                conn.Open();
                string sql = "UPDATE TestAccount SET Balance = Balance - 0.1 WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            conn.Close();
            UserActionMune(ID);
            return true;
        }

        public bool Transfer(int ID)
        {
            conn = new SqlConnection(connStr);
            if (conn != null)
            { conn.Close(); }
                Console.WriteLine("Please enter the transfer userid ：");
            int otherID = Convert.ToInt32(Console.ReadLine()); 
            if (Check1(otherID)==false)
            {
                Console.WriteLine("User does not exist：");
                if (conn != null)
                { conn.Close(); }
                return false;
            }
            Console.WriteLine("Please enter the transfer amount：");
            amount = decimal.Parse(Console.ReadLine());
            if (Check(ID,amount)==false)
            {
                Console.WriteLine("Insufficient balance！");                
                return false;
            }
            else
            {
                try
                {
                    conn = new SqlConnection(connStr);                    
                    conn.Open();
                    string sql1 = "UPDATE TestAccount SET Balance = Balance +" + amount + " WHERE CustomerID =  " + otherID + " " ;
                    SqlCommand cmd1 = new SqlCommand(sql1, conn);
                    dr = cmd1.ExecuteReader();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                try
                {
                    conn = new SqlConnection(connStr);
                    conn.Open();
                    string sql = "UPDATE TestAccount SET Balance = Balance - " + amount + " WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";                  
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    dr = cmd.ExecuteReader();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                try
                {
                    conn = new SqlConnection(connStr);
                    conn.Open();
                    string sql = "UPDATE TestAccount SET Balance = Balance - 0.2 WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    dr = cmd.ExecuteReader();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                conn.Close();
                UserActionMune(ID);             
                return true;
            }
        }
        
        public Boolean DisplayCustomer(int ID)
        {
            string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
            SqlConnection conn = null;
            SqlDataReader dr = null;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from TestCustomer where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID"));
                    string Name = reader.GetString(reader.GetOrdinal("Name"));
                    string Address = reader.GetString(reader.GetOrdinal("Address"));
                    string City = reader.GetString(reader.GetOrdinal("City"));
                    string PostCode = reader.GetString(reader.GetOrdinal("PostCode"));
                    Console.Write("CustomerID:{0},Name:{1},Address:{2},City:{3},PostCode:{4}}\n", CustomerID, Name, Address, City, PostCode);
                }

            }
            catch (Exception e)
            { Console.WriteLine(e); }           
            UserActionMune(ID);
            return true;
        }
        public void DisplayAccount(int ID)
        {
            string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
            SqlConnection conn = null;
            SqlDataReader dr = null;
            if (conn != null)
            { conn.Close(); }
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from TestAccount where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int AccountNumber = reader.GetInt32(reader.GetOrdinal("AccountNumber"));
                    string AccountType = reader.GetString(reader.GetOrdinal("AccountType"));
                    int CustomerID = reader.GetInt32(reader.GetOrdinal("CustomerID"));
                    decimal Balance = reader.GetDecimal(reader.GetOrdinal("Balance"));
                    Console.Write("AccountNumber:{0},AccountType:{1},CustomerID:{2},Balance:{3}\n", AccountNumber, AccountType, CustomerID, Balance);
                }
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            if (conn != null)
            { conn.Close(); }            
            UserActionMune(ID);
        }
        public void DisplayTransaction(int ID)
        {
            string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
            SqlConnection conn = null;
            SqlDataReader dr = null;
            if (conn != null)
            { conn.Close(); }
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM TestTransaction where AccountNumber = ( select AccountNumber from TestAccount where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + "))";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    int TransactionID = reader.GetInt32(reader.GetOrdinal("TransactionID"));
                    string TransactionType = reader.GetString(reader.GetOrdinal("TransactionType"));
                    int AccountNumber = reader.GetInt32(reader.GetOrdinal("AccountNumber"));
                    int DestinationAccountNumber = reader.GetInt32(reader.GetOrdinal("DestinationAccountNumber"));
                    decimal Amount = reader.GetDecimal(reader.GetOrdinal("Amount"));
                    string Comment = reader.GetString(reader.GetOrdinal("Comment"));
                    string TransactionTimeUtc = reader.GetString(reader.GetOrdinal("TransactionTimeUtc"));
                    Console.Write("TransactionID:{0},TransactionType:{1},AccountNumber:{2},DestinationAccountNumber:{3},DestinationAccountNumber:{4},Amount{5},Comment:{6},TransactionTimeUtc:{7}\n", TransactionID, TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc);
                }

            }
            catch (Exception e)
            { Console.WriteLine(e); }
            if (conn != null)
            { conn.Close(); }
            UserActionMune(ID);
        }
        public bool Check(int ID , decimal amount)
        {
            string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
            SqlConnection conn = null;
            SqlDataReader dr = null;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Close();
                conn.Open();
                string sql = "select * from TestAccount where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlCommand cmd = new SqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
                Console.WriteLine(dr);
                if (dr.Read())
                {
                    balance= Convert.ToInt32(dr[0].ToString());
                    if (balance > amount) 
                    { return true; }
                }
                else
                {  return false;}
            }
            catch (Exception e)
            {  Console.WriteLine(e);}
            if (conn != null)
            { conn.Close(); }
            return false;
        }
        public bool Check1(int otherID)
        {
            string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
            SqlConnection conn = null;
            SqlDataReader dr = null;
            if (conn != null)
            { conn.Close(); }
            try
            {
                conn = new SqlConnection(connStr);                
                conn.Open();
                string sql = "select * from TestAccount where CustomerID = " + otherID + " ";
                SqlCommand cmd = new SqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
                if (dr!=null)
                { return true; }
                else
                { return false; }
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            if (conn != null)
            { conn.Close(); }
            return false;
        }
    }
}
