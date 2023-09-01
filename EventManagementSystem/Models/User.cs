using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public Guid EventId { get; set; }
         public Event Event { get; set; }
    }
}
