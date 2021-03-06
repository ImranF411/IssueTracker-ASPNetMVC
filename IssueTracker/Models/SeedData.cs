using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using IssueTracker.Data;
using IssueTracker.Enums;
using System;
using System.Linq;

namespace IssueTracker.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new IssueTrackerContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<IssueTrackerContext>>()))
            {
                User admin = new User
                {
                    Name = "Adam Minn",
                    Username = "Admin",
                    IsActive = true,
                    Role = "Admin"
                };

                if (!context.Users.Contains(admin)) { context.Users.Add(admin); }

                User normalUser = new User
                {
                    Username = "user",
                    Name = "Norma L. Euseur",
                    IsActive = true,
                    Role = "User"
                };

                if (!context.Users.Contains(normalUser)) { context.Users.Add(normalUser); }

                if (context.Tickets.Any())
                {
                    return;
                }

                context.Tickets.AddRange(
                    new Ticket
                    {
                        Subject = "There is a critical bug.",
                        CreationDate = DateTime.Now,
                        Creator = admin,
                        Assignee = admin,
                        SeverityLevel = SeverityEnum.Critical,
                        Status = StatusEnum.AwaitingCustomerResponse,
                        Description = "This should be top priority"
                    },

                    new Ticket
                    {
                        Subject = "There is a less important bug.",
                        CreationDate = new DateTime(1993,4,11),
                        Creator = normalUser,
                        Assignee = admin,
                        SeverityLevel = SeverityEnum.Low,
                        Status = StatusEnum.Investigation,
                        Description = "This should be low priority"
                    },

                     new Ticket
                     {
                         Subject = "There is a normal important bug.",
                         CreationDate = new DateTime(2022,2, 17),
                         Creator = normalUser,
                         Assignee= normalUser,
                         SeverityLevel = SeverityEnum.Normal,
                         Status = StatusEnum.New,
                         Description = "This should be middle priority"
                     },
                      new Ticket
                      {
                          Subject = "There is a very important bug.",
                          CreationDate = new DateTime(2022,2,18),
                          Creator = normalUser,
                          Assignee = normalUser,
                          SeverityLevel = SeverityEnum.Important,
                          Status = StatusEnum.InDevelopment,
                          Description = "This should be high priority"
                      }
                );
                context.SaveChanges();
            }    
        }
    }
}
