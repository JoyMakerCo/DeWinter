using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
    public class AfterPartySoundMediator : MonoBehaviour
    {
        //To Do: We need some kind of short, relaxing after party theme or musical sting
        public FMODEvent AmbientSFX;
        // Use this for initialization
        void Awake()
        {
            AmbitionApp.SendMessage(AudioMessages.PLAY_AMBIENTSFX, AmbientSFX);
            AmbitionApp.SendMessage(AudioMessages.STOP_MUSIC);
        }
    }
}
