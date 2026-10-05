using MediatR;
using DevFreela.Core.Repositories;
using DevFreela.Core.DTOs;
using DevFreela.Core.Services;
using DevFreela.Core.Enums;
namespace DevFreela.Application.Commands.FinishProject
{
  public class FinishProjectCommandHandler : IRequestHandler<FinishProjectCommand, FinishProjectResult>
  {
    private readonly IProjectRepository _projectRepository;
    private readonly IPaymentService _paymentService;
    public FinishProjectCommandHandler(IProjectRepository projectRepository, IPaymentService paymentService)
    {
      _projectRepository = projectRepository;
      _paymentService = paymentService;
    }
    public async Task<FinishProjectResult> Handle(FinishProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      if (project == null)
      {
        return FinishProjectResult.ProjectNotFound;
      }
      // Only work in progress can be paid for; this also stops a second payment
      // request while the first one is still PaymentPending.
      if (project.Status != ProjectStatusEnum.InProgress)
      {
        return FinishProjectResult.ProjectNotInProgress;
      }

      // Persist PaymentPending before publishing, so the payment-approved
      // consumer never reads the project while it is still InProgress.
      project.SetPaymentPending();
      await _projectRepository.SaveChangesAsync();

      var paymentInfoDTO = new PaymentInfoDTO(request.Id, request.CreditCardNumber, request.Cvv, request.ExpiresAt, request.FullName, project.TotalCost);

      _paymentService.ProcessPayment(paymentInfoDTO);

      return FinishProjectResult.PaymentRequested;
    }
  }
}