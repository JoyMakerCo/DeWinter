using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Remark {

    public int toneInt;
    public string tone;
    public int targetingProfileInt;
    public int[] targetingProfile;
    public bool ambushRemark; //Creates a black 'Dead' Remark for when Players are Ambushed

    //Randomly Generates a Remark, this will be used when the Player has a Remark added to their hand
    public Remark()
    {
        toneInt = Random.Range(0, 4);
        tone = GameData.dispositionList[toneInt].like; //Randomly takes a tone for the Remark from the disposition list
        GenerateTargettingProfile();
        ambushRemark = false;
    }

    public Remark(string info)
    {
        if (info == "ambush") {
            ambushRemark = true;
        }
        else
        {
            if (info == "Boast")
            {
                RandomExclusiveTone(0);
            }
            else if (info == "Intrigue")
            {
                RandomExclusiveTone(1);
            }
            else if (info == "Flattery")
            {
                RandomExclusiveTone(2);
            }
            else
            {
                RandomExclusiveTone(3);
            }
        }
        GenerateTargettingProfile();
    }
	
    void GenerateTargettingProfile()
    {
        int profileInt = Random.Range(1, 6);
        switch (profileInt)
        {
            case 1:
                targetingProfileInt = 1;
                targetingProfile = new int[1];
                targetingProfile[0] = 1;
                break;
            case 2:
                targetingProfileInt = 1;
                targetingProfile = new int[1];
                targetingProfile[0] = 1;
                break;
            case 3:
                targetingProfileInt = 11;
                targetingProfile = new int[2];
                targetingProfile[0] = 1;
                targetingProfile[1] = 1;
                break;
            case 4:
                targetingProfileInt = 101;
                targetingProfile = new int[3];
                targetingProfile[0] = 1;
                targetingProfile[1] = 0;
                targetingProfile[2] = 1;
                break;
            case 5:
                targetingProfileInt = 1011;
                targetingProfile = new int[4];
                targetingProfile[0] = 1;
                targetingProfile[1] = 0;
                targetingProfile[2] = 1;
                targetingProfile[3] = 1;
                break;
        }
    }

    //Randomly Selects a Tone EXCEPT for the one entered
    void RandomExclusiveTone(int exclusiveToneInt)
    {     
        List<int> randomIntList = new List<int>();
        for (int i = 0; i < 4; i++)
        {
            if (i != exclusiveToneInt)
            {
                randomIntList.Add(i);
            }
        }
        int listRandom = Random.Range(0, randomIntList.Count);
        toneInt = randomIntList[listRandom];
        tone = GameData.dispositionList[toneInt].like;       
    }
}
