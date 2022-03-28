using System;
using System.Collections.Generic;
using DevFreela.Core.Entities;

namespace DevFreela.Infrastructure.Persistence
{
  public class DevFreelaDbContext
  {
    public DevFreelaDbContext()
    {
      Projects = new List<Project>
      {
        new Project("My frist project", "My project description 1", 1, 1, 1000),
        new Project("My second project", "My project description 2", 1, 1, 2000),
        new Project("My third project", "My project description 3", 1, 1, 3000),
      };
      Users = new List<User>
      {
        new User("Jose da Silva","sjose@gmal.com", new DateTime(1999,02,21)),
        new User("Robert Black","robert@gmal.com", new DateTime(1982,06,31)),
        new User("Charles Graice","charles@gmal.com", new DateTime(1999,12,11)),
      };
      Skills = new List<Skill>
      {
        new Skill(".NET CORE"),
        new Skill("Dapper"),
        new Skill("Oracle DB"),
      };
    }
    public List<Project> Projects { get; set; }
    public List<User> Users { get; set; }
    public List<Skill> Skills { get; set; }
  }
}