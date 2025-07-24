using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Payment
{
    public interface IPayOsService
    {
        Task<CreatePaymentResult> CreatePaymentLinkAsync(PaymentData paymentData);
        Task<PaymentLinkInformation> GetPaymentLinkInformationAsync(long orderId);
        WebhookData VerifyPaymentWebhookData(WebhookType webhookBody);
    }
}
