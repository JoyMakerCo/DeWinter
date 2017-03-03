using UnityEngine;
using System.Collections;

public class Day {

    public int month; // Which number month is this?
    public int day; // Which day in the month is this? (zero indexed)
    public int displayDay; //Which day is this? Publicly shown
    public int linearDayValue; //Is this the 99th day? The 100th?
    public Party party1;
    public Party party2;

    //To Do: There needs to be a set cadence initially for parties (one on this day but not that day), but this current method of if statements and direct assignments is clunky and inelegant
    public Day(int dDay, int dMonth) 
    {
        day = dDay;
        displayDay = day + 1;
        month = dMonth;
        MakeParties();       
    }

    void MakeParties()
    {
        if (month == 0 && day == 0)
        {
            party1 = new Party(0);//No Party on the first day
            party2 = new Party(0);
        }
        else if (month == 0 && day == 1)
        {
            party1 = new Party(1); // Guaranteed Tiny Party
            party2 = new Party(0);
        }
        else if (month == 0 && day == 4)
        {
            party1 = new Party(1); // Guaranteed Tiny Party
            party2 = new Party(0);
        }
        else
        {
            party1 = new Party(Random.Range(0, 4));
            if (Random.Range(1, 4) == 1)
            {
                party2 = new Party(party1.faction);
            }
            else
            {
                party2 = new Party(0);
            }
        }
    }

}
