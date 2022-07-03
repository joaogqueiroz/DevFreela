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
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _paymentBaseUrl;
    public PaymentService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
      _httpClientFactory = httpClientFactory;
      _paymentBaseUrl = configuration.GetSection("Services:Payments").Value;
    }
    public async Task<bool> ProcessPayment(PaymentInfoDTO paymentInfoDTO)
    {
      var url = $"{_paymentBaseUrl}/api/payments";
      var paymentInfoJson = JsonSerializer.Serialize(paymentInfoDTO);

      var paymentInfoContent = new StringContent(
        paymentInfoJson,
        Encoding.UTF8,
        "application/json"
      );
      var httpClient = _httpClientFactory.CreateClient("Payments");
      var respose = await httpClient.PostAsync(url, paymentInfoContent);
      //need to implement payment gateway
      return respose.IsSuccessStatusCode;
    }

  }
}