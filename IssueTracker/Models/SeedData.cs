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
                    Name = "Adadm Minn",
                    IsActive = true,
                    Role = "Admin"
                };

                if (!context.Users.Contains(admin)) { context.Users.Add(admin); }

                User normalUser = new User
                {
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
                        Name = "There is a critical bug.",
                        CreationDate = DateTime.Now,
                        Creator = admin,
                        SeverityLevel = SeverityEnum.Critical,
                        Status = StatusEnum.AwaitingCustomerResponse
                    },

                    new Ticket
                    {
                        Name = "There is a less important bug.",
                        CreationDate = new DateTime(1993,4,11),
                        Creator = normalUser,
                        SeverityLevel = SeverityEnum.Low,
                        Status = StatusEnum.Investigation
                    },

                     new Ticket
                     {
                         Name = "There is a normal important bug.",
                         CreationDate = new DateTime(2022,2, 17),
                         Creator = normalUser,
                         SeverityLevel = SeverityEnum.Normal,
                         Status = StatusEnum.New
                     },
                      new Ticket
                      {
                          Name = "There is a very important bug.",
                          CreationDate = new DateTime(2022,2,18),
                          Creator = normalUser,
                          SeverityLevel = SeverityEnum.Important,
                          Status = StatusEnum.InDevelopment
                      }
                );
                context.SaveChanges();
            }    
        }
    }
}
