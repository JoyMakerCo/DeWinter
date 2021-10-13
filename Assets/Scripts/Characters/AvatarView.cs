using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class AvatarView : MonoBehaviour
	{
        public Animator animator;
        public Image Art;

        private string _pose;
        private AvatarConfig _avatar;

        public AvatarConfig Avatar
        {
            get => _avatar;
            set
            {
                _avatar = value;
                Pose = _pose;
            }
        }

        public string ID => Avatar?.name;

		public string Pose
		{
			get => _pose;
			set
			{
                Sprite spt = Avatar?.GetPose(_pose = value);
                Art.enabled = (spt != null);
                if (spt != null) Art.overrideSprite = spt;
            }
		}

        public void Play(CharacterMotion motion)
        {
            animator.SetTrigger(motion.ToString());
        }

        public Sprite Sprite => Art?.overrideSprite;
	}
}
