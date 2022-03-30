using System;
using System.Collections.Generic;
using System.Linq;
namespace DevFreela.Application.InputModels
{
  public class CreateUserInputModel
  {
    public CreateUserInputModel(string fullName, string passWord, string email, DateTime birthDate)
    {
      this.FullName = fullName;
      this.PassWord = passWord;
      this.Email = email;
      this.BirthDate = birthDate;

    }
    public string FullName { get; set; }
    public string PassWord { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool Active { get; set; }
  }
}