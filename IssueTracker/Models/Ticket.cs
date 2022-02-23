using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IssueTracker.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        
        [Required]
        public string? Subject { get; set; }

        [Required]
        public string? Description { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        
        public User? Creator { get; set; }

        [Display(Name = "Assigned To")] 
        public User? Assignee { get; set; }
        
        public SeverityEnum? SeverityLevel { get; set; }
        
        public StatusEnum? Status { get; set; }

        [NotMapped]
        public List<SelectListItem> SeverityLevelList { get; } = new List<SelectListItem> {
            new SelectListItem { Text = "Critical", Value = SeverityEnum.Critical.ToString() },
            new SelectListItem { Text = "Important", Value = SeverityEnum.Important.ToString() },
            new SelectListItem { Text = "Normal", Value = SeverityEnum.Normal.ToString()},
            new SelectListItem { Text = "Low", Value = SeverityEnum.Low.ToString() }
        };

        [NotMapped]
        public List<SelectListItem> StatusList { get; } = new List<SelectListItem> {
            new SelectListItem { Text = "New", Value = StatusEnum.New.ToString() },
            new SelectListItem { Text = "Investigation", Value = StatusEnum.Investigation.ToString() },
            new SelectListItem { Text = "Closed", Value = StatusEnum.Closed.ToString()},
            new SelectListItem { Text = "Hold", Value = StatusEnum.Hold.ToString() },
            new SelectListItem { Text = "Awaiting Customer Response", Value = StatusEnum.AwaitingCustomerResponse.ToString() },
            new SelectListItem { Text = "In Development", Value = StatusEnum.InDevelopment.ToString() }
        };
    }
}
