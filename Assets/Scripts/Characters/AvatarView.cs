using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class AvatarView : MonoBehaviour
	{
		public AvatarCollection Collection;

		private AvatarVO _avatar;
		private Image _avatarImage;
		private string _id;
		private string _pose;

		void Awake()
		{
			_avatarImage = gameObject.GetComponent<Image>();
		}

		public string AvatarID
		{
			get { return _id; }
			set {
				_id = value;
				_avatar = Collection.GetAvatar(_id);
				gameObject.SetActive(!default(AvatarVO).Equals(_avatar));
				Pose = _pose;
			}
		}

		public string Pose
		{
			get { return _pose; }
			set
			{
				_pose = value;
				_avatarImage.sprite = _avatar.GetPose(_pose);
			}
		}
	}
}
