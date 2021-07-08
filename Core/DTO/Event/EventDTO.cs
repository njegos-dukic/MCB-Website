using System;
using System.Collections.Generic;
using System.Text;

namespace Core.DTO
{
    public class EventDTO
    {
        public int EventId { get; set; }

        public string Organizer { get; set; }

        public DateTime BeginOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int DurationInHours { get; set; }

        public double Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public bool IsUpcoming { get; }

        public EventDTO()
        {
            IsUpcoming = BeginOn.CompareTo(DateTime.UtcNow) >= 0;
        }
    }
}
