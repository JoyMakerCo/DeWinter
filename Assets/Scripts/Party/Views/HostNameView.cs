using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class HostNameView : MonoBehaviour
	{
		void Start ()
		{
			PartyModel model = AmbitionApp.GetModel<PartyModel>();
			NotableVO host = model.Party.Host;
			this.gameObject.GetComponent<Text>().text = host.Name;
		}
	}
}
