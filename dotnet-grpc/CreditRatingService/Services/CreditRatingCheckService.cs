using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace CreditRatingService
{
    public class CreditRatingCheckService : CreditRatingCheck.CreditRatingCheckBase
    {
        private readonly ILogger<CreditRatingCheckService> _logger;

        private static readonly Dictionary<string, Int32> customerTrustedCredit = new Dictionary<string, Int32>() 
        {
            {"arnold", 10000},
            {"alex", 5000},
            {"lena", 2500},
            {"vany", 2500}
        };
        public CreditRatingCheckService(ILogger<CreditRatingCheckService> logger)
        {
            _logger = logger;
        }

        public override Task<CreditReply> 
            CheckCreditRequest(CreditRequest request, ServerCallContext context)
        {
            return Task.FromResult(
                new CreditReply{
                    IsAccepted = IsEligibleForCredit(request.CustomerId, request.Credit)
                }
            );
        }
        private bool IsEligibleForCredit(string customerId, Int32 credit) {
            bool isEligible = false;

            if (customerTrustedCredit.TryGetValue(customerId, out Int32 maxCredit))
            {
                isEligible = credit <= maxCredit;
            }

            return isEligible;
        }
    }
}
