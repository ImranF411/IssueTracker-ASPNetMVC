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
        public string? Name { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime CreationDate { get; set; }
        public string? Creator { get; set; }
        public SeverityEnum? SeverityLevel { get; set; }

        [NotMapped]
        public List<SelectListItem> SeverityLevelList { get; } = new List<SelectListItem> {
            new SelectListItem { Text = "Critical", Value = SeverityEnum.Critical.ToString() },
            new SelectListItem { Text = "Important", Value = SeverityEnum.Important.ToString() },
            new SelectListItem { Text = "Normal", Value = SeverityEnum.Normal.ToString(), Selected = true },
            new SelectListItem { Text = "Low", Value = SeverityEnum.Low.ToString() }
        };
    }
}
