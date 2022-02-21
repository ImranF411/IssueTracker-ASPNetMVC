using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models

{
    public class User
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }

        public string? Role { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object? obj)
        {
            if(obj == null)
                return false;
            if(obj.GetType() != typeof(User))
                return false; 
            return this.Id == ((User)obj).Id;
        }
    }
}
