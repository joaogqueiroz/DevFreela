using DevFreela.Core.Services;
using DevFreela.Core.DTOs;
using System.Threading.Tasks;
namespace DevFreela.Infrastructure.Payments
{
  public class PaymentService : IPaymentService
  {
    public Task<bool> ProcessPayment(PaymentInfoDTO paymentInfoDTO)
    {
      //need to implement payment gateway
      return Task.FromResult(true);
    }

  }
}