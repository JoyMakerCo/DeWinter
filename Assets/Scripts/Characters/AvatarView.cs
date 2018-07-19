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
		private string _id;
		private string _pose;

		void Awake()
		{
			_image = gameObject.GetComponent<Image>();
		}

		public string ID
		{
			get { return _id; }
			set {
				_id = value;
				Avatar = Collection.GetAvatar(_id);
				Pose = _pose;
			}
		}

		public string Pose
		{
			get { return _pose; }
			set
			{
				_pose = value;
				_image.sprite = Avatar.GetPose(_pose);
                _image.enabled = (_image.sprite != null);
			}
		}

        public Sprite Sprite
        {
            get { return _image.sprite;  }
        }
	}
}
