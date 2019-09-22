using System;
namespace Ambition
{
    public interface ICalendarEvent
    {
        DateTime Date { set;  get; }
        string Name { get; }
        bool IsComplete { set; get; }
    }
}
