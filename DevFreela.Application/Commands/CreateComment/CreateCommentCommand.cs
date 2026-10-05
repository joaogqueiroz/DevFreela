using System;
using System.Linq;
using MediatR;
using System.Threading.Tasks;
namespace DevFreela.Application.Commands.CreateComment
{
  public class CreateCommentCommand : IRequest<bool>
  {
    public string Content { get; set; }
    public int IdProject { get; set; }
    public int IdUser { get; set; }
  }
}