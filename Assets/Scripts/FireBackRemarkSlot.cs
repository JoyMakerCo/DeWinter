using UnityEngine;
using System.Collections;

public class FireBackRemarkSlot {

        //General Settings
        public int dispositionInt;
        public Disposition disposition;
        public bool dispositionRevealed;
        public int lockedInState = 0; //0 for Active, 1 for Charmed and -1 for Put Out

        //Generates a random regular Guest
        public FireBackRemarkSlot()
        {
            dispositionInt = Random.Range(0, 4);
            disposition = GameData.dispositionList[dispositionInt];
            dispositionRevealed = false;
            lockedInState = 0;     
        }
}

