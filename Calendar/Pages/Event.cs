using Microsoft.Extensions.Logging;

namespace Calendar.Pages
{
    public class Event
    {
        public DateTime StartDate;
        public DateTime EndDate;
        private static int _nextId = 1;

        public int Id { get; set; } = _nextId++;
        public string Title { get; set; }
        public string Description { get; set; }
        public Day SelectedDay { get; set; }
        public int EventResource { get; set; }
        public bool Accepted { get; set; } = false;

    }
}