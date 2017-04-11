using System;
using System.Collections;
using System.Collections.Generic;

public class Remark
{
    public Disposition Disposition;
    public BitArray Profile;

    // Todo: Generate this stuff in a command
    public Remark(string info, int numGuests)
    {
		if (info != "ambush")
        {
			switch(info)
			{
            	case "Politics":
					Profile = RandomExclusiveTone(0);
					break;
				case "Theater":
					Profile = RandomExclusiveTone(1);
					break;
				case "Gossip":
					Profile = RandomExclusiveTone(2);
					break;
				default:
					Profile = RandomExclusiveTone(3);
					break;
            }
        }
        else
        {
        	Profile = (BitArray)0;
        }
		GenerateTargettingProfile(numGuests);
    }
	
	void GenerateTargettingProfile(int numGuests) //Guest Number is the number of Guests in the Room, which determines the targetting profiles available
    {
    	Profile = (BitArray)(1+(2*(new Random().Next(Math.Pow(2,numGuests-1)))));
    }

    //Randomly Selects a Tone EXCEPT for the one entered
	Disposition RandomExclusiveTone(int exclusiveToneInt)
    {
		int index = new Random(1, GameData.dispositionList.Length);
		return (index != exclusiveToneInt) ? GameData.dispositionList[index] : GameData.dispositionList[0];
	}
}
