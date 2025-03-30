using CashFlow.Domain.Enums;
using CashFlow.Domain.Reports;

namespace CashFlow.Domain.Extensions
{
    public static class PaymentTypeExtension
    {
        public static string PaymentTypeToString(this PaymentType paymentType)
        {
            return paymentType switch
            {
                PaymentType.Cash => ResourceReportGenerationMessage.CASH,
                PaymentType.CreditCard => ResourceReportGenerationMessage.CREDITCARD,
                PaymentType.DebitCard => ResourceReportGenerationMessage.DEBITCARD,
                PaymentType.EletronicTransfer => ResourceReportGenerationMessage.ELETRONICTRANSFER,
                _ => string.Empty
            };
        }
    }
}
