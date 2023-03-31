using System;
namespace demoapp.Models
{
	public class Attending
	{

        public int Id { get; set; }

        public int UserId { get; set; }

        public int EventId { get; set; }

        public Boolean IsAttending { get; set; }

        public string? Notes { get; set; }

        public DateTime TimeStamp { get; set; }

    }
}

