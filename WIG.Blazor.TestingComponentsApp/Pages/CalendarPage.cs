namespace WIG.Blazor.TestingComponentsApp.Pages
{
    public partial class CalendarPage
    {
        public bool verticalmonth = true;
        public bool horizontalmonth = false;
        public bool horizontalyear = false;
        public bool horizontalmonthextended = false;

        public void ChangeView()
        {
            if (verticalmonth)
            {
                verticalmonth = false;
                horizontalyear = false;
                horizontalmonth = true;
            }
            else if (horizontalyear && !horizontalmonth)
            {
                horizontalmonth = false;
                verticalmonth = false;
                horizontalyear = false;
                horizontalmonthextended = true;
            }
            else if (horizontalmonth && !horizontalyear)
            {
                horizontalmonth = false;
                verticalmonth = false;
                horizontalyear = true;
            }
            else if (horizontalmonthextended && !verticalmonth)
            {
                horizontalmonth = false;
                verticalmonth = true;
                horizontalyear = false;
                horizontalmonthextended = false;

            }
        }
    }
}
