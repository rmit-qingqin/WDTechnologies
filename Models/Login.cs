namespace WebServiceAndDatabaseExample
{
    public class Login
    {
        public string LoginID { get; }
        public int CustomerID { get; }
        public string PasswordHash { get; }

        public Login(string loginID, int customerID, string passwordHash)
        {
            loginID = loginID;
            customerID = customerID;
            passwordHash = passwordHash;
        }
    }
}