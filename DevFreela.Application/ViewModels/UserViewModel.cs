using System;
using System.Collections.Generic;
namespace DevFreela.Application.ViewModels
{
  public class UserViewModel
  {
    public UserViewModel(string fullName, string email, DateTime birthDate)
    {
      this.FullName = fullName;
      this.Email = email;
      this.BirthDate = birthDate;

    }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }

  }
}