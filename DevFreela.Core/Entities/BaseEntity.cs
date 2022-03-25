using System;
using System.Collections.Generic;
namespace DevFreela.Core.Entities
{
  public abstract class BaseEntity
  {
    protected BaseEntity() { }
    public int Id { get; private set; }
  }
}