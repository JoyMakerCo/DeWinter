using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class DrinkBoozeBtnView : MonoBehaviour
	{
		private Image _btnImage;
		void Awake ()
		{ 
			_btnImage = gameObject.GetComponent<Image>();
			AmbitionApp.Subscribe<int>(GameConsts.INTOXICATION, HandleDrank);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(GameConsts.INTOXICATION, HandleDrank);
		}

		private void HandleDrank(int tox)
		{
			_btnImage.color =  (tox > 0) ? Color.white : Color.gray;
		}
	}
}