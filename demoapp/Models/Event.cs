using System;
namespace demoapp.Models
{
	public class Event
	{
        public int Id { get; set; }

        public string? Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? TimeZone { get; set; }

        public string? Description { get; set; }

        public User? User { get; set; }


    }
}

