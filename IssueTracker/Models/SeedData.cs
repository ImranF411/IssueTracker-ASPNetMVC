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
                if(context.Tickets.Any())
                {
                    return;
                }

                context.Tickets.AddRange(
                    new Ticket
                    {
                        Name = "There is a critical bug.",
                        CreationDate = DateTime.Now,
                        Creator = "John Smith",
                        SeverityLevel = SeverityEnum.Critical
                    },

                    new Ticket
                    {
                        Name = "There is a less important bug.",
                        CreationDate = new DateTime(1993,4,11),
                        Creator = "Beeg Oohfs",
                        SeverityLevel = SeverityEnum.Low
                    },

                     new Ticket
                     {
                         Name = "There is a normal important bug.",
                         CreationDate = new DateTime(2022,2, 17),
                         Creator = "Asd If",
                         SeverityLevel = SeverityEnum.Normal
                     },
                      new Ticket
                      {
                          Name = "There is a very important bug.",
                          CreationDate = new DateTime(2022,2,18),
                          Creator = "Mana Manuh",
                          SeverityLevel = SeverityEnum.Important
                      }
                );
                context.SaveChanges();
            }    
        }
    }
}
