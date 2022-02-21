﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace IssueTracker.Models
{
    public class FilterViewModel
    {
        public List<Ticket>? Tickets { get; set; }
        public SelectList? Creators { get; set; }
        public string? Creator { get; set; }
        public string? SearchString { get; set; }
    }
}
