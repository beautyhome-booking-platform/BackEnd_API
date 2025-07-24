using Net.payOS.Types;
using Net.payOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payment
{
    public class PayOsService : IPayOsService
    {
        private readonly PayOS _payOS;

        public PayOsService(PayOS payOS)
        {
            _payOS = payOS;
        }

        public async Task<CreatePaymentResult> CreatePaymentLinkAsync(PaymentData paymentData)
        {
            return await _payOS.createPaymentLink(paymentData);
        }

        public async Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(long orderId)
        {
            return await _payOS.getPaymentLinkInformation(orderId);
        }

        public WebhookData VerifyPaymentWebhookData(WebhookType webhookBody)
        {
            return _payOS.verifyPaymentWebhookData(webhookBody);
        }
    }
}
