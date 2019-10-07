using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Models;
using System.Text; 

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentGatewayController : ControllerBase
    {
        static List<PaymentBankTransaction> allTransactionDetails = new List<PaymentBankTransaction>(){};
        private readonly ILogger<PaymentGatewayController> _logger;

        public PaymentGatewayController(ILogger<PaymentGatewayController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<PaymentBankTransaction> Get()
        {
            return allTransactionDetails;
        }

        [HttpGet]
        [Route("/get/[controller]/{param}")] 
        public List<PaymentBankTransaction> Get(int param)
        {
            List<PaymentBankTransaction> myTransactionDetails = new List<PaymentBankTransaction>(){};
            foreach (var merchantDetails in allTransactionDetails) {
                if (merchantDetails.merchantID == param) {
                    string protectedCardNumber = "- "+merchantDetails.cardNumber.Substring(merchantDetails.cardNumber.Length - 3);
                    PaymentBankTransaction resultDetails = new PaymentBankTransaction{
                        transactionID = merchantDetails.transactionID,
                        merchantID = merchantDetails.merchantID,
                        cardNumber = protectedCardNumber,
                        expiryDetails = merchantDetails.expiryDetails,
                        transactionType = merchantDetails.transactionType,
                        transactionStatus = merchantDetails.transactionStatus,
                        transactionAmount = merchantDetails.transactionAmount,
                        cvv = merchantDetails.cvv
                    };
                    myTransactionDetails.Add(resultDetails);
                }
                   
                }
            return myTransactionDetails; 
           
        }

        [Route("/post/[controller]")]
        public string Post([FromBody]PaymentMerchantTransaction value)
        {
            string errorMsg = "";

            Random random = new Random();  
            int newTransactionID = random.Next(1000);

            PaymentBankTransaction t1 = new PaymentBankTransaction();
            t1.transactionID = newTransactionID;
            t1.merchantID = value.merchantID;
            t1.cardNumber = value.cardNumber;
            t1.expiryDetails = value.expiryDetails;
            t1.transactionType = value.transactionType;
            t1.transactionAmount = value.transactionAmount;
            t1.cvv = value.cvv;

            if (value.merchantID != 0)
            {
                t1.transactionStatus = "success";
                allTransactionDetails.Add(t1);
                errorMsg = "Transaction successful";
            }
            else {
                //do not do any thing
                t1.transactionStatus = "failure";
                allTransactionDetails.Add(t1);
                errorMsg = "An error occured. Information missing";
            }    
            return errorMsg;            
        }

    }
}
