using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class EstateSoundMediator : MonoBehaviour
    {

        //TO DO: Make the mediator check to see which chapter the game is in, and adjust the theme as needed
        public FMODEvent AmbientSFX;
        //public FMODEvent YvetteTheme; <-Waiting on Yvette's theme musics to be finished for each chapter

        // Use this for initialization
        void Awake()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFX);
            //AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, YvetteTheme);
        }

    }
}
