using System;
using System.ComponentModel.DataAnnotations;

namespace EventManagementSystem.Models
{
    public class Event
    {
        [Key]
        public Guid EventId { get; set; }
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string Description { get; set; } = string.Empty;
        
        [Required]
        public string Location { get; set; } = string.Empty;
        
        [Required]
        [Range(1, int.MaxValue)]
        public int Capacity { get; set; }
        
        [Required]
        [Range(0, double.MaxValue)]
        public double TicketAmount { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        
        public ICollection<User> RegisteredUsers { get; set; }
    }
}
