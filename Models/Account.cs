namespace WebServiceAndDatabaseExample
{
    public class Account
    {
        public int AccountNumber { get; }
        public string AccountType { get; }
        public int CustomerID { get; }
        public decimal Balance { get; }

        public Account(int accountNumber, string accountType, int customerID, decimal balance)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            CustomerID = customerID;
            Balance = balance;
        }
    }
}