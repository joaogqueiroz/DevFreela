using System;
using System.Collections.Generic;
namespace DevFreela.Core.Exceptions
{
  public class ProjectAlredyStartedExcepetion : Exception
  {
    public ProjectAlredyStartedExcepetion() : base("Project is already in Started status")
    {

    }
  }
}