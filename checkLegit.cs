using Microsoft.Extensions.Configuration;

namespace WebServiceAndDatabaseExample
{
    public class checkLegit
    {
        string accountType;
        decimal balance;
        public  checkLegit(string accountType, decimal balance ) {
            accountType = accountType;
            balance = balance;
        }
        public bool check() {
            if (accountType == "c") {
                if (balance >= 200)
                {
                    return true;
                }
                else {
                    return false;
                }
                
            }
            if ((accountType == "s"))
            {
                if (balance >= 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }return false;

        }
    }
}
