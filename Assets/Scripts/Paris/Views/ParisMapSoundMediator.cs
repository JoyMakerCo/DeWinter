using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class ParisMapSoundMediator : MonoBehaviour
    {
        public FMODEvent AmbientSFX;
        // Use this for initialization
        void Awake()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFX);    
        }
    }
}

