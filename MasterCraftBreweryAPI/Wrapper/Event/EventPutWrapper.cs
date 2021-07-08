using System;

namespace MasterCraftBreweryAPI.Wrapper.Event
{
    public class EventPutWrapper
    {
        /// <summary>
        /// Unique identifier for the event
        /// </summary>
        public int EventId { get; set; }

        /// <summary>
        /// Organizer of the event
        /// </summary>
        public string Organizer { get; set; }

        /// <summary>
        /// When does the event begin?
        /// </summary>
        public DateTime BeginOn { get; set; }

        /// <summary>
        /// When does the event end?
        /// </summary>
        public DateTime? EndOn { get; set; }

        /// <summary>
        /// Duration of the event in hours
        /// </summary>
        public int DurationInHours { get; set; }

        /// <summary>
        /// Price for attending the event
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Title of the event
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description for the event
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Location of the event
        /// </summary>
        public string Location { get; set; }
    }
}
