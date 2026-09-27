using DevFreela.Core.DTOs;
using System.Threading.Tasks;
namespace DevFreela.Core.Services
{
  public interface IPaymentService
  {
    void ProcessPayment(PaymentInfoDTO paymentInfoDTO);
  }
}