using System;
using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public class VolumeSlider : MonoBehaviour
    {
        public Slider Slider;
        public AudioChannel Channel;

        void OnEnable()
        {
            Slider.value = AmbitionApp.GetService<AudioSvc>().GetVolume(Channel);
        }

        public void OnVolumeChanged()
        {    
            AmbitionApp.GetService<AudioSvc>().SetVolume(Channel, (int)Slider.value);
        }
    }
}
