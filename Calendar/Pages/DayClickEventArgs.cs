using Microsoft.AspNetCore.Components.Web;

namespace Calendar.Pages;

public class DayClickEventArgs
{
    public Day Day { get; set; }
    public MouseEventArgs MouseEventArgs { get; set; }
}
