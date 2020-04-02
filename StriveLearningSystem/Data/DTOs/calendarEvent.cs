using System;
using System.Collections.Generic;
using System.Text;

namespace Data.DTOs
{
    // based on https://fullcalendar.io/docs/event-object
    public class CalendarEvent
    {
        public string Id { get; set; }
        public string GroupId { get; set; }
        public bool AllDay { get; set; } = false;
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string[] ClassNames { get; set; }
        public List<int> DaysOfWeek { get; set; }
    }
}
