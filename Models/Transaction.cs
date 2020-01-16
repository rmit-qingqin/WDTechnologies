namespace WebServiceAndDatabaseExample
{
    public class Transaction
    {
        public int TransactionID { get; }
        public string TransactionType { get; }
        public int AccountNumber { get; }
        public int DestinationAccountNumber { get; }
        public decimal Amount { get; }
        public string Comment { get; }
        public string TransactionTimeUtc { get; }

        public Transaction(int transactionID ,string transactionType ,int accountNumber,int destinationAccountNumber , decimal amount, string comment, string transactionTimeUtc)
        {
            TransactionID = transactionID;
            TransactionType = transactionType;
            AccountNumber = accountNumber;
            DestinationAccountNumber = destinationAccountNumber;
            Amount = amount;
            Comment = comment;
            TransactionTimeUtc = transactionTimeUtc;
        }
    }
}