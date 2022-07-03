using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using DevFreela.Core.Repositories;
using DevFreela.Core.DTOs;
using DevFreela.Core.Services;
namespace DevFreela.Application.Commands.FinishProject
{
  public class FinishProjectCommandHandler : IRequestHandler<FinishProjectCommand, bool>
  {
    private readonly IProjectRepository _projectRepository;
    private readonly IPaymentService _paymentService;
    public FinishProjectCommandHandler(IProjectRepository projectRepository, IPaymentService paymentService)
    {
      _projectRepository = projectRepository;
      _paymentService = paymentService;
    }
    public async Task<bool> Handle(FinishProjectCommand request, CancellationToken cancellationToken)
    {
      var project = await _projectRepository.GetByIdAsync(request.Id);
      project.Finish();

      var paymentInfoDTO = new PaymentInfoDTO(request.Id, request.CreditCardNumber, request.Cvv, request.ExpiresAt, request.FullName, project.TotalCost);
      var result = await _paymentService.ProcessPayment(paymentInfoDTO);

      if (!result)
        project.SetPaymentPending();

      await _projectRepository.SaveChangesAsync();
      return result;
    }
  }
}