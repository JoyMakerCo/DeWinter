using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Calendar {

    public List<Month> monthList = new List<Month>();

    public Calendar()
    {
        stockWithMonths();
        Debug.Log("Created Calendar!");
    }

    //TODO - The month system only displays correctly because the game reads the start position in the week of the first day every month. This feels artificial and janky. There has to be a more elegant way to do this.
    void stockWithMonths()
    {
        //"Month Name", Number of Days in Month, Month Number, Starting Position of Month Days, Number of Weeks in the Month
        monthList.Add(new Month("Janvier", 31, 0, 0, 4));
        monthList.Add(new Month("Fevrier", 28, 1, startingDayPos(31, monthList[0].startingDayPosition), 4));
        monthList.Add(new Month("Mars", 31, 2, startingDayPos(28, monthList[1].startingDayPosition), 4));
        monthList.Add(new Month("Avril", 30, 3, startingDayPos(31, monthList[2].startingDayPosition), 6));
        monthList.Add(new Month("Mai", 31, 4, startingDayPos(30, monthList[3].startingDayPosition), 5));
        monthList.Add(new Month("Juin", 30, 5, startingDayPos(31, monthList[4].startingDayPosition), 4));
        monthList.Add(new Month("Juillet", 31, 6, startingDayPos(30, monthList[5].startingDayPosition), 4));
        monthList.Add(new Month("Aout", 31, 7, startingDayPos(31, monthList[6].startingDayPosition), 4));
        monthList.Add(new Month("Septembre", 30, 8, startingDayPos(31, monthList[7].startingDayPosition), 4));
        monthList.Add(new Month("Octobre", 31, 9, startingDayPos(30, monthList[8].startingDayPosition), 4));
        monthList.Add(new Month("Novembre", 31, 10, startingDayPos(31, monthList[9].startingDayPosition), 4));
        monthList.Add(new Month("Decembre", 31, 11, startingDayPos(31, monthList[10].startingDayPosition), 4));
    }

    //TODO - Make this able to check multiple months in the future or the past (low priority for now)
    public Day daysFromNow(int dayNumber)
    {
        if(dayNumber == 0)
        {
            return today();
        } else
        {
            if (GameData.currentDay + dayNumber <= 0) //If the requested day is in a past Month
            {
                int day = (monthList[GameData.currentMonth - 1].days - (GameData.currentDay + dayNumber)); // Fix this
                return monthList[GameData.currentMonth - 1].SelectDayByInt(day);
            }
            else if (GameData.currentDay + dayNumber <= monthList[GameData.currentMonth].days) //If the requested day is in the current Month
            {
                return monthList[GameData.currentMonth].SelectDayByInt(GameData.currentDay + dayNumber);
            }
            else //If the requested day is in a future Month
            {
                int day = (monthList[GameData.currentMonth].days - GameData.currentDay) + dayNumber;
                return monthList[GameData.currentMonth + 1].SelectDayByInt(day);
            }
        } 
    }

    public Day today()
    {
        return monthList[GameData.currentMonth].SelectDayByInt(GameData.currentDay);
    }

    public Day yesterday()
    {
        return daysFromNow(-1);
    }

    int startingDayPos(int prevMonthLength, int prevMonthStartPos)
    {
        return (prevMonthLength + prevMonthStartPos) % 7;
    }
}
