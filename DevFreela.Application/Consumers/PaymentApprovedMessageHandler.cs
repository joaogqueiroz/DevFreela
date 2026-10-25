using DevFreela.Core.IntegrationEvents;
using DevFreela.Core.Repositories;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevFreela.Application.Consumers
{
    // Decides what a payment-approved message leads to, without any RabbitMQ code,
    // so it can be tested on its own.
    public class PaymentApprovedMessageHandler
    {
        private readonly IProjectRepository _projectRepository;

        public PaymentApprovedMessageHandler(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        // Returns false when the message cannot be read, has no valid project id or names a
        // project that does not exist: none of these can succeed on a retry.
        public async Task<bool> HandleAsync(string message)
        {
            PaymentApprovedIntegrationEvent? paymentApproved;
            try
            {
                paymentApproved = JsonSerializer.Deserialize<PaymentApprovedIntegrationEvent>(message);
            }
            catch (JsonException)
            {
                return false;
            }

            // A missing IdProject is read as 0, which is never a real project
            if (paymentApproved == null || paymentApproved.IdProject <= 0)
            {
                return false;
            }

            var project = await _projectRepository.GetByIdAsync(paymentApproved.IdProject);
            if (project == null)
            {
                return false;
            }

            // Finish() only acts on PaymentPending, so a duplicate approval changes nothing
            project.Finish();
            await _projectRepository.SaveChangesAsync();
            return true;
        }
    }
}
