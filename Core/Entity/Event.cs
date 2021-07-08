using System;

namespace Core.Entity
{
    public class Event
    {
        public int EventId { get; set; }

        public int CompanyId { get; set; }

        public string Organizer { get; set; }

        public DateTime BeginOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int DurationInHours { get; set; }

        public string Location { get; set; }

        public double Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string PhotoUri { get; set; }

        #region NavigationProperties

        public Company Company { get; set; }
        #endregion
    }
}
