using System.Globalization;

namespace Calendar.Pages
{
    public class Day
    {
        public DateTime Date { get; set; }
        public int? DayNumber { get; set; }
        public string Class { get; set; }
        public bool IsOtherMonth { get; set; }
        public bool IsCurrentDay => Date == DateTime.Today;
        public bool IsWeekend => (Date.DayOfWeek == DayOfWeek.Saturday || Date.DayOfWeek == DayOfWeek.Sunday);
    }
}
