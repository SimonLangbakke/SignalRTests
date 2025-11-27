using Stripe;

namespace Events.Application.Contracts.ExternalApis;

public interface IPaymentService
{
    Task<PaymentIntent> CreatePaymentIntent(long amount, Dictionary<string, string> metadata);
}