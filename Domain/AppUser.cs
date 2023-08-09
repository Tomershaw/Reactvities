using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class AppUser :IdentityUser
    {
        public string DisplayName {get; set; }
        public string Bio { get; set; }
        public bool CanCreateActivity{get; set;}
        public ICollection<ActivityAttendee> Activities {get; set;}
        public ICollection<Photo> photos {get; set;}
    }
}