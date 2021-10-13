using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
    public class BarkView : MonoBehaviour
    {
        public Text BarkText;
        public Animation Animator;
        public Image Glow;
        public Image Icon;

        public float Duration => Animator.clip.length;
        public bool IsComplete => Animator.isPlaying;

        public void SetBark(string text, Color color, Sprite icon)
        {
            BarkText.text = text;
            BarkText.color = color;
            Glow.color = color;
            Icon.sprite = icon;
            Icon.gameObject.SetActive(icon != null);
            gameObject.SetActive(true);
            Animator.Rewind("Reward_Bark");
            Animator.Play("Reward_Bark");
        }
    }
}
