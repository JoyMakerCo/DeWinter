using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class AvatarView : MonoBehaviour
	{
		public AvatarCollection Collection;
        public AvatarVO Avatar { private set; get; }

        private Image _image;
		private string _pose;

		void Awake()
		{
			_image = gameObject.GetComponent<Image>();
		}

		public string ID
		{
            get { return Avatar.ID; }
			set {
                Avatar = Collection.GetAvatar(value);
				Pose = _pose;
			}
        }

		public string Pose
		{
			get { return _pose; }
			set
			{
                Sprite spt = Avatar.GetPose(_pose = value);
                if (_image == null) Awake();
                _image.enabled = (spt != null);
                if (spt != null) _image.sprite = spt;
            }
		}

        public Sprite Sprite
        {
            get { return _image.sprite;  }
        }
	}
}
