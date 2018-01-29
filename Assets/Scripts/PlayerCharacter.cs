using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter {

    string characterName;
    string characterDescription;

    public PlayerCharacter(string n, string desc)
    {
        characterName = n;
        characterDescription = desc;
    }

    public string name
    {
        get { return characterName; }
    }

    public string description
    {
        get { return characterDescription;  }
    }
}
