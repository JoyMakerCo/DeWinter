using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    //This is for all of the  various scenes that need their own music and ambient sfx handled (The Estate Screen, Le Petit Mogul, etc...)
    public class SceneSoundMediator : MonoBehaviour
    {
        public FMODEvent AmbientSFX;
        public FMODEvent Music;
        
        // Use this for initialization
        void Awake()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENT, AmbientSFX);
            if (Music.Name.Length > 0)
            {
                AmbitionApp.SendMessage(AudioMessages.PLAY_MUSIC, Music);
            } else
            {
                AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
            }
        }
    }
}