namespace Calendar.Pages
{
    //class week to create the weeks of the month
    public class Week
    {
        public int WeekNumber { get; set; }
        public List<Day> Days { get; } = new List<Day>();
    }
}
