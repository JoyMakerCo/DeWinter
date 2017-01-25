using System;
using UnityEngine;
using System.Collections.Generic;

namespace DeWinter
{
	public class StartupManager : MonoBehaviour
	{
		public void Start ()
		{
			DeWinterApp.RegisterModel<DevotionModel>();
			DeWinterApp.RegisterModel<FactionModel>();

			GameObject.Destroy(this.gameObject);
		}
	}
}

