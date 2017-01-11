using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Month {
    public string name;
    public int days; //Number of Days in the Month
    public int weeks; // Number of Weeks the Month occupies in the Displayed Calendar in the Estate Screen
    public int monthNumber;
    public int startingDayPosition; // Which day does the Month start at? Sunday = 0, Monday = 1, etc...
    public Day[,] dayList = new Day[6, 7];
    //public Day[][] dayList; 
    //public List<Day> dayList = new List<Day>();

    // Fully Featured Constructor
    public Month(string n, int d, int mN, int sDP, int w)
    {
        name = n;
        days = d;
        monthNumber = mN;
        weeks = w;
        startingDayPosition = sDP;
        stockWithDays();
    }

    // Blank/Throw away Constructor
    public Month()
    {
        name = "None";
        days = 0;
        stockWithDays();
    }

    void stockWithDays()
    {
        int row = 0;
        int column = 0;
        int dayCount = 0;
        for (int i = 0; i < 42; i++)
        {
            //This section determines whether days get placed
            if (i < startingDayPosition)
            {
                dayList[row,column] = null;
            } else if ( i >= days+startingDayPosition){
                dayList[row,column] = null;
            } else
            {
                dayList[row,column] = new Day(dayCount, monthNumber);
                dayCount++;
            }
            //This section advances the rows and columns
            if (column < 6)
            {
                column++;
            } else
            {
                column = 0;
                row++;
            }
        }
    }

    public Day SelectDayByInt(int dayNumber)
    {
        int dayCount = 0;
        int row = 0;
        int column = 0;
        while (dayCount < dayNumber)
        {
            //This section advances the rows and columns
            if (column < 6)
            {
                column++;
            }
            else
            {
                column = 0;
                row++;
            }
            //This section determines whether day count goes up
            if (dayList[row,column] != null)
            {
                dayCount++;
            }     
        }
        return dayList[row,column];
    }
}
