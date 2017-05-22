using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class HostTelegraphTextView : MonoBehaviour
	{
		private Text _text;
		NotableVO _host;

		void Awake()
		{
			AmbitionApp.Subscribe<GuestVO>(PartyMessages.TELEGRAPH, HandleTelegraph);
			AmbitionApp.Subscribe<GuestVO>(PartyMessages.GUEST_REMARK, HandleRemark);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.TELEGRAPH, HandleTelegraph);
			AmbitionApp.Unsubscribe<GuestVO>(PartyMessages.GUEST_REMARK, HandleRemark);
		}

		void Start()
		{
			_host = AmbitionApp.GetModel<PartyModel>().Party.Host;
			_text = gameObject.GetComponent<Text>();
		}

		// None of this should be necessary if the game ends when the host is locked in...
		private void HandleTelegraph(GuestVO guest)
		{
			this.gameObject.SetActive(true);
			if (guest == _host)
			{
				_text.text = _host.Name + " is about to speak...";
			}
        }

		private void HandleRemark(GuestVO guest)
		{
			if (guest == _host)
			{
				gameObject.SetActive(false);
			}
		}
	}
}
