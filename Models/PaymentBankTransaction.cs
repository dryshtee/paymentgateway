namespace PaymentGateway.Models
{
    public class PaymentBankTransaction
    {
        public int transactionID {set;get;}
        public int merchantID {set;get;}
        public string cardNumber {set;get;}
        public string expiryDetails {set;get;}
        public string transactionType {set;get;}
        public double transactionAmount {set;get;}
        public string transactionStatus {set;get;}
        public int cvv {set;get;}
    }
}