using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class PlayerNameTextView : MonoBehaviour
	{
		void Start ()
		{
			Text text = gameObject.GetComponent<Text>();
			text.text = "Yvette";
		}
	}
}