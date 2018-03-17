using System.Collections;
using UnityEngine;

namespace Ambition
{
    public class Shuffle : MonoBehaviour
    {
        public AmbientClip[] Clips;

        void OnEnable()
        {
            Play();
        }

        public void Play()
        {
            int index = Util.RNG.Generate(Clips.Length);
            AmbitionApp.SendMessage<AmbientClip>(AudioMessages.PLAY_MUSIC, Clips[index]);
        }
    }
}
