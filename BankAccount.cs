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
        string connStr = "Server=wdt2020.australiasoutheast.cloudapp.azure.com;Database=s3712674;Uid=s3712674;Password=abc123";
        SqlConnection conn = null;
        SqlDataReader dr = null;
        SqlDataReader dr1 = null;
        string strt = DateTime.Now.ToLocalTime().ToString("dd/MM/yyyy");

        public BankAccount()
        { }
       
        public void UserActionMune(int ID)
        {
            conn = new SqlConnection(connStr);
            conn.Open();
            while (true)
            {
                Console.WriteLine("Please choose your function: ");
                Console.Write("1.Deposit 2.Withdraw 3.Transfer 4.DisplayTransaction 5.Customer Modify information 6.Log out 7.Exit ");
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
                        conn.Close();
                        Menu meu = new Menu();
                        meu.Run();
                        break;
                    case 7:
                        conn.Close();
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
                string sql1 = "UPDATE TestAccount SET Balance = Balance - 0.1 WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlCommand cmd = new SqlCommand(sql1, conn);
                dr1 = cmd.ExecuteReader();
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
            Console.WriteLine("Please enter the transfer userid ：");
            int otherID = Convert.ToInt32(Console.ReadLine()); 
            if (Check1(otherID)==false)
            {
                Console.WriteLine("User does not exist：");
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
                    string sql2 = "UPDATE TestAccount SET Balance = Balance - " + amount + " WHERE CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";                  
                    SqlCommand cmd2 = new SqlCommand(sql2, conn);
                    dr1 = cmd2.ExecuteReader();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                try
                {
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
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from TestCustomer where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(" CustomerID: "+reader[" CustomerID"].ToString() + "\r");
                    Console.WriteLine("Name: " + reader["Name"].ToString() + "\r");
                    Console.WriteLine("Address: " + reader["Address"].ToString() + "\r");
                    Console.WriteLine("City: " + reader["City"].ToString() + "\r");
                    Console.WriteLine("PostCode: " + reader["PostCode"].ToString() + "\r");
                    Console.WriteLine("State: " + reader["State"].ToString() + "\r");
                    Console.WriteLine("Phone: " + reader["Phone"].ToString() + "\r");
                }
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("Please enter the name ：");
            String custName = Convert.ToString(Console.ReadLine());
            try
            {
                SqlCommand cmd0 = conn.CreateCommand();
                cmd0.CommandText = "UPDATE TestCustomer	SET Name =  "+ custName + " where CustomerID  where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader0 = cmd0.ExecuteReader();             
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("Please enter the address ：");
            String custAddress = Convert.ToString(Console.ReadLine());
            try
            {
                SqlCommand cmd1 = conn.CreateCommand();
                cmd1.CommandText = "UPDATE TestCustomer	SET Address =  " + custAddress + " where CustomerID  where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader1 = cmd1.ExecuteReader();
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("Please enter the city ：");
            String custCity = Convert.ToString(Console.ReadLine());
            try
            {
                SqlCommand cmd2 = conn.CreateCommand();
                cmd2.CommandText = "UPDATE TestCustomer	SET City =  " + custCity + " where CustomerID  where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader2 = cmd2.ExecuteReader();
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("Please enter the PostCode ：");
            int custPostcode = Convert.ToInt32(Console.ReadLine());
            while (custPostcode < 0 || custPostcode > 99999) 
            {
                Console.WriteLine("Please enter a number in the range ：");
                Console.WriteLine("Please enter the PostCode ：");
                custPostcode = Convert.ToInt32(Console.ReadLine());
            }
            try
            {
                SqlCommand cmd3 = conn.CreateCommand();
                cmd3.CommandText = "UPDATE TestCustomer	SET PostCode =  " + custPostcode + " where CustomerID  where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader3 = cmd3.ExecuteReader();
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("Please enter the State ：");
            String custState = Convert.ToString(Console.ReadLine());
            try
            {
                SqlCommand cmd4 = conn.CreateCommand();
                cmd4.CommandText = "UPDATE TestCustomer	SET State =  " + custState + " where CustomerID  where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader4 = cmd4.ExecuteReader();
            }
            catch (Exception e)
            { Console.WriteLine(e); }
            Console.WriteLine("Please enter the custPhone ：");
            long custPhone = Convert.ToInt64(Console.ReadLine());
            while (custPhone < 1000000000 || custPhone > 9999999999)
            {
                Console.WriteLine("Please enter a number in the range ：");
                Console.WriteLine("Please enter the custPhone ：");
                custPhone = Convert.ToInt64(Console.ReadLine());
            }
            try
            {
                SqlCommand cmd5 = conn.CreateCommand();
                cmd5.CommandText = "UPDATE TestCustomer	SET Phone =  " + custPhone + " where CustomerID  where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader5 = cmd5.ExecuteReader();
            }
            catch (Exception e)
            { Console.WriteLine(e); }            
            conn.Close();
            UserActionMune(ID);
            return true;
        }
        public void DisplayAccount(int ID)
        {
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "select * from TestAccount where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + ")";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("TransactionID: "+ reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("TransactionType: " + reader["TransactionType"].ToString() + "\r");
                    Console.WriteLine("AccountNumber: " + reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("DestinationAccountNumber: " + reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("Amount: " + reader["TransactionType"].ToString() + "\r");
                }
            }
            catch (Exception e)
            { Console.WriteLine(e); }  
            conn.Close();
            UserActionMune(ID);
        }
        public void DisplayTransaction(int ID)
        {
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT * FROM TestTransaction where AccountNumber = ( select AccountNumber from TestAccount where CustomerID = (select CustomerID from TestLogin where LoginID = " + ID + "))";
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("TransactionID: " + reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("TransactionType: " + reader["TransactionType"].ToString() + "\r");
                    Console.WriteLine("AccountNumber: " + reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("DestinationAccountNumber: ", reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("Amount: " + reader["TransactionType"].ToString() + "\r");
                    Console.WriteLine("Comment: " + reader["TransactionID"].ToString() + "\r");
                    Console.WriteLine("ModifyDate: " + reader["TransactionType"].ToString() + "\r");
                }

            }
            catch (Exception e)
            { Console.WriteLine(e); }
            conn.Close();
            UserActionMune(ID);
        }
        public bool Check(int ID , decimal amount)
        {
            try
            {
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
            return false;
        }
        public bool Check1(int otherID)
        {
            try
            {
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
            return false;
        }
    }
}
