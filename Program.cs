using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
namespace WebServiceAndDatabaseExample
{
    public class Menu
    {
        private static IConfigurationRoot Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        private static string ConnectionString { get; } = Configuration["ConnectionString"];
        public int ID;
        string connStr = ConnectionString ;
        SqlConnection conn = null;
        SqlDataReader dr = null;
        string strt = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy");

        public void Run()
        {
            conn = new SqlConnection(connStr);
            conn.Open();
            Console.WriteLine("Hello!");
            Console.WriteLine("Please choose your function: ");
            Console.WriteLine("1.Login 2.Register 3.Exit");
            var option = Convert.ToString(Console.ReadLine());
            //Console.WriteLine("your input is "+option);
            switch (option)
            {
                case "1":
                    login();
                    break;
                case "2":
                    register();
                    break;
                case "3":
                    System.Environment.Exit(0);
                    break;
            }
        }
        protected void login()
        {
            Console.WriteLine("Please enter your ID: ");
            ID =Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Please enter your password: ");
            String password = Convert.ToString(Console.ReadLine());
            if (Check(ID, password) == true)
            {   
                Console.WriteLine("Login success!");
                BankAccount a = new BankAccount();
                a.UserActionMune(ID);
                }
            else
            {
                Console.WriteLine("Wrong ID or password!");
                login();
            }
        }
        public int Getid()
        {
            int id = ID; 
            return id; 
        }
        public bool Check(int ID, String password)
        {
            string PasswordHash;
            try
            {                
                string sql = "select PasswordHash from TestLogin where LoginID = " + "'" + ID + "'";
                SqlCommand cmd = new SqlCommand(sql, conn);
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    PasswordHash = dr[0].ToString();
                    PBKDF2 pkbdf2 = new PBKDF2(64, 32, 50000);
                    var verify = pkbdf2.verifyHash(password, PasswordHash);
                    return verify;                    
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            conn.Close();
            return false;
        }
        protected void register()
        {
            try
            {
                Console.WriteLine("please choose account type: 1.Savings 2.Checkings");
                var option = Convert.ToInt32(Console.ReadLine());
                var accountType = " ";
                if (option == 1)
                {
                    accountType = "s";
                }
                else if (option == 2)
                {
                    accountType = "c";
                }
                else
                {
                    Console.WriteLine("please choose a right option!");
                    register();
                }
                Console.WriteLine("Please enter your customer name: ");
                String customerName = Convert.ToString(Console.ReadLine());
                Console.WriteLine("Please enter your password: ");
                String password = Convert.ToString(Console.ReadLine());
                PBKDF2 pkbdf2 = new PBKDF2(64, 32, 50000);
                String passwordHash = pkbdf2.createHash(password);
                Console.WriteLine("Please enter your cutomerID with 4 numbers: ");
                int customerID = Convert.ToInt32(Console.ReadLine());
                while (customerID < 0 || customerID > 9999)
                {
                    Console.WriteLine("Please enter a number in the range 0000--9999");
                    Console.WriteLine("Please enter your cutomerID with 4 numbers: ");
                    customerID = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine("Please enter your accountnumber with 4 numbers: ");
                int accountNumber = Convert.ToInt32(Console.ReadLine());
                while (accountNumber < 0 || accountNumber > 9999)
                {
                    Console.WriteLine("Please enter a number in the range 0000--9999");
                    Console.WriteLine("Please enter your accountnumber with 4 numbers: ");
                    accountNumber = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine("Please enter your login ID with 8 numbers: ");
                int loginID = Convert.ToInt32(Console.ReadLine());
                while (loginID < 0 || loginID > 99999999)
                {
                    Console.WriteLine("Please enter a number in the range 0--99999999");
                    Console.WriteLine("Please enter your login ID with 8 numbers: ");
                    loginID = Convert.ToInt32(Console.ReadLine());
                }
                Console.WriteLine("Please enter your balance with numbers: ");
                decimal balance = Convert.ToInt32(Console.ReadLine());
                balance = checkBalance(accountType, balance);
                if (checkReg(customerID, accountNumber, loginID) == true)
                {
                    //add data to database
                    /*
                    using var connection = ConnectionString.CreateConnection();
                    connection.Open();
                    var command = connection.CreateCommand();
                    var b = command.CommandText =
                        "insert into TestLogin (loginID, CustomerID, PasswordHash, ModifyDate) values" + "(" + "'" + loginID + "'," + "'" + customerID + "'," + "'" + passwordHash + "'"+"'," + "'" + strt + "'" + ")";
                    command.ExecuteNonQuery();
                    using var connection1 = ConnectionString.CreateConnection();
                    connection1.Open();
                    var command1 = connection.CreateCommand();
                    var a = command1.CommandText =
                        "insert into TestAccount(AccountNumber, AccountType, CustomerID, Balance, ModifyDate) values" + "(" + "'" + accountNumber + "'," + "'" + accountType + "'," + "'" + customerID + "'," + "'" + balance + "'" + "'," + "'" + strt + "'" + ")";
                    command1.ExecuteNonQuery();
                    */
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "insert into TestLogin (loginID, CustomerID, PasswordHash, ModifyDate) values" + "(" + "'" + loginID + "'," + "'" + customerID + "'," + "'" + passwordHash + "'" + "'," + "'" + strt + "'" + ")";
                        SqlDataReader reader = cmd.ExecuteReader();

                    }
                    catch (Exception e)
                    { Console.WriteLine(e); }
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "insert into TestAccount(AccountNumber, AccountType, CustomerID, Balance, ModifyDate) values" + "(" + "'" + accountNumber + "'," + "'" + accountType + "'," + "'" + customerID + "'," + "'" + balance + "'" + "'," + "'" + strt + "'" + ")";
                        SqlDataReader reader = cmd.ExecuteReader();

                    }
                    catch (Exception e)
                    { Console.WriteLine(e); }
                    try
                    {
                        SqlCommand cmd = conn.CreateCommand();
                        cmd.CommandText = "insert into TestCustomer( CustomerID ) values" + "(" + "'" + customerID + "'" + ")";
                        SqlDataReader reader = cmd.ExecuteReader();

                    }
                    catch (Exception e)
                    { Console.WriteLine(e); }
                    
                    Run();
                }
                else
                {
                    Console.WriteLine("please input new inforamtion or input right length of information!");
                    //return register
                    register();
                }
                // create a new account and send it to database
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
        private decimal checkBalance(string accountType,decimal balance) 
        {
            if (accountType == "a")
            {
                if (balance < 100)
                {
                    Console.WriteLine("Please enter your balance higher than 100: ");
                    balance = Convert.ToInt32(Console.ReadLine());
                    return checkBalance(accountType, balance);
                }
                else
                {
                    return balance;
                }
            }
            else 
            {
                if (balance < 500)
                {
                    Console.WriteLine("Please enter your balance higher than 500: ");
                    balance = Convert.ToInt32(Console.ReadLine());
                    return checkBalance(accountType, balance);
                }
                else 
                {
                    return balance;

                }
            }

        }
        private bool checkReg(int customerID, int accountNumber, int loginID)
        {
            if (customerID.ToString().Length == 4 && accountNumber.ToString().Length == 4 && loginID.ToString().Length == 8)
            {
                // get data from database and check
                string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
                SqlConnection conn = null;
                SqlDataReader dr = null;
                try
                {
                    conn = new SqlConnection(connStr);
                    conn.Open();
                    string sql = "select CustomerID from TestLogin where LoginID = " + loginID;
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        for (int i = 0; i < 999; i++) {
                            if (customerID.ToString() == dr[i].ToString())
                            {
                                return false;
                            }
                            else 
                            {
                                return true;
                            }
                        }

                    }
                    else
                    {
                        return true;

                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
