using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
namespace Calendar.Pages
{
    public partial class MonthCalendar
    {
       

        //label for the month and year in the calendar header
        public string CurrentMonthYear => _currentMonth.ToString("MMMM yyyy");
        public string NextMonthYear => _currentMonth.AddMonths(1).ToString("MMMM yyyy");

        [Parameter]
        public EventCallback<SchedulePeriod> SchedulePeriodClick { get; set; }


        //list of weeks for the calendar
        public List<Week> CurrentMonthWeeks { get; set; }
        public List<Week> NextMonthWeeks { get; set; }

        public Day? SelectedStartDay { get; set; }
        public Day? SelectedEndDay { get; set; }

        //represents the current month
        private DateTime _currentMonth;

        public string SelectionMessage  { get; set; }
        public string SelectionMessageStyle  { get; set; }

/* methods */

        protected override void OnInitialized()
        {
            //initializing variables for the calendar the events and the calendar
            _currentMonth = DateTime.Today;
            //InitializeEvents();
            GenerateCalendar();

        }

        //method to create the calendar (days and weeks)
        private void GenerateCalendar()
        {
            // Initialize variables for the current month
            CurrentMonthWeeks = new List<Week>();
            GenerateMonthCalendar(_currentMonth, CurrentMonthWeeks);

            // Initialize variables for the next month
            NextMonthWeeks = new List<Week>();
            var nextMonth = _currentMonth.AddMonths(1);
            GenerateMonthCalendar(nextMonth, NextMonthWeeks);
        }

        private void GenerateMonthCalendar(DateTime month, List<Week> weeksList)
        {
            var daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            var currentDay = 1;

            // Start a new week
            var week = new Week();
            AddEmptyDaysToStartOfMonth(week, month);

            // Add days of the month
            while (currentDay <= daysInMonth)
            {
                if (week.Days.Count == 7)
                {
                    weeksList.Add(week);
                    week = new Week();
                }

                var day = new Day
                {
                    Date = new DateTime(month.Year, month.Month, currentDay),
                    DayNumber = currentDay,
                    IsOtherMonth = false,
                };
                day.Class = GetDayClass(day);

                week.Days.Add(day);
                currentDay++;
            }

            // Add empty days to last week until it has 7 days
            AddEmptyDaysToEndOfMonth(week, month);
            weeksList.Add(week);
        }

        private string GetDayClass(Day day)
        {
            var classes = "";

            // Check if the day is the current day
            if (day.IsCurrentDay)
            {
                classes += "current-day ";
            }

            // Check if the day is a weekend
            if (day.IsWeekend)
            {
                classes += "weekend-day ";
            }
            // Check if the day is the selected start day
            else if (SelectedStartDay != null && day.Date == SelectedStartDay.Date)
            {
                classes += "selected-day ";
            }
            // Check if the day is within the selected date range and not a weekend
            else if (SelectedStartDay != null && SelectedEndDay != null)
            {
                if (day.Date >= SelectedStartDay.Date && day.Date <= SelectedEndDay.Date)
                {
                    classes += "selected-day ";
                }
            }

            // Add any existing classes from the day object
            classes += day.Class;

            return classes.Trim();
        }


        private void AddEmptyDaysToStartOfMonth(Week week, DateTime month)
        {
            var firstDayOfMonth = new DateTime(month.Year, month.Month, 1);
            var firstDayOfWeekIndex = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;

            for (int i = 0; i < firstDayOfWeekIndex; i++)
            {
                var prevMonthDays = DateTime.DaysInMonth(month.AddMonths(-1).Year, month.AddMonths(-1).Month);
                var day = new Day
                {
                    DayNumber = null, //prevMonthDays - (firstDayOfWeekIndex - i - 1),
                    Date = new DateTime(month.AddMonths(-1).Year, month.AddMonths(-1).Month, prevMonthDays - (firstDayOfWeekIndex - i - 1)),
                    Class = "prev-month ",
                    IsOtherMonth = true,
                };

                if (day.Date.DayOfWeek == DayOfWeek.Saturday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    day.Class += "weekend-day ";
                }

                week.Days.Add(day);
            }
        }


        private void AddEmptyDaysToEndOfMonth(Week week, DateTime month)
        {
            int nextDays = 1;
            while (week.Days.Count < 7)
            {
                var day = new Day
                {
                    DayNumber = null, //nextDays,
                    Class = "next-month ",
                    Date = new DateTime(month.AddMonths(1).Year, month.AddMonths(1).Month, nextDays),
                    IsOtherMonth = true,
                };

                if (day.Date.DayOfWeek == DayOfWeek.Saturday || day.Date.DayOfWeek == DayOfWeek.Sunday)
                {
                    day.Class += "weekend-day ";
                }

                week.Days.Add(day);
                nextDays++;
            }
        }


        //method to skip to the next month
        private void NextMonth()
        {
            _currentMonth = _currentMonth.AddMonths(1);
            GenerateCalendar();
        }
        //method to skip to the previous month
        private void PreviousMonth()
        {
            _currentMonth = _currentMonth.AddMonths(-1);
            GenerateCalendar();
        }

        // Method to show the popup with the current day
        private void MonthDayClicked(DayClickEventArgs dayClickEventArgs)
        {
            var day = dayClickEventArgs.Day;
            var mouseEvents = dayClickEventArgs.MouseEventArgs;
            if (!(day.IsOtherMonth || day.IsWeekend))
            {
                CalculateDateRange(day);
                SetSelectionMessage(mouseEvents);
                Console.WriteLine($"SelectedStartDay: {SelectedStartDay?.Date}");
                Console.WriteLine($"SelectedEndDay: {SelectedEndDay?.Date}");
                GenerateCalendar();
            }
        }

        private void SetSelectionMessage(MouseEventArgs e)
        {
            // Calculate the style to position the div below the clicked point
            // Adjust the offset as needed to position the div correctly

            if (SelectedStartDay != null && SelectedEndDay != null)
            {
                TimeSpan difference = SelectedEndDay.Date - SelectedStartDay.Date;
                int numberOfDays = difference.Days + 1;
                SelectionMessage = $"Marcar {SelectedStartDay.DayNumber} de {SelectedStartDay.Date.ToString("MMMM")} a {SelectedEndDay.DayNumber} de {SelectedEndDay.Date.ToString("MMMM")} ({numberOfDays} dias)";
                SelectionMessageStyle = $"left: {e.ClientX - 400}px; top: {e.ClientY + 30}px;";
                Console.WriteLine($"SelectionMessage: {SelectionMessage}");
                Console.WriteLine($"SelectionMessageStyle: {SelectionMessageStyle}");
            } else
            {
                SelectionMessage = "";
                SelectionMessageStyle = "";
            }
            
        }

        private void CalculateDateRange(Day clickedDay)
        {
            // If no start date is selected, or both start and end dates are already selected, reset and set new start date
            if (SelectedStartDay == null || (SelectedStartDay != null && SelectedEndDay != null))
            {
                SelectedStartDay = clickedDay;
                SelectedEndDay = null; // Reset end date
            }
            else if (SelectedEndDay == null)
            {
                // Ensure that the end date is after the start date
                if (clickedDay.Date >= SelectedStartDay.Date)
                {
                    SelectedEndDay = clickedDay;
                }
                else
                {
                    // If the clicked day is before the start date, reset the start date
                    SelectedStartDay = clickedDay;
                }
            }

            // Update the UI or perform further actions
            StateHasChanged();
        }

        public void OnSchedulePeriodClick()
        {
            var schedulePeriod = new SchedulePeriod()
            {
                StartDate = SelectedStartDay.Date,
                EndDate = SelectedEndDay.Date
            };

            SchedulePeriodClick.InvokeAsync(schedulePeriod);

        }

        [Parameter]
        public Action<Event> EventClick { get; set; }
        //method to show the details of the event clicked
        private void EventDetails(Event ev)
        {
            EventClick.Invoke(ev);
        }


    }
}
