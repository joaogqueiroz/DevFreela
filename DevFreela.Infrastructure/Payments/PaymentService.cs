using DevFreela.Core.Services;
using DevFreela.Core.DTOs;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
namespace DevFreela.Infrastructure.Payments
{
  public class PaymentService : IPaymentService
  {
    private readonly IMessageBusService _messageBusService;
    private const string QUEUE_NAME = "Payments";
    public PaymentService(IMessageBusService messageBusService)
    {
      _messageBusService = messageBusService;
    }
    public void ProcessPayment(PaymentInfoDTO paymentInfoDTO)
    {

      var paymentInfoJson = JsonSerializer.Serialize(paymentInfoDTO);
      var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJson);

      _messageBusService.Publish(QUEUE_NAME, paymentInfoBytes);
    }
  }
}