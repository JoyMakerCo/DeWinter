using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class HostViewMediator : MonoBehaviour
	{
		// Host
		public GameObject HostContainer;
		public Image HostPortrait;
		public Text HostNameText;
		public TimerView HostTelegraphBar;
		public Text HostTelegraphText;

		private PartyModel _model;

		private NotableVO Host 
		{
			get { return _model.Party.Host; }
		}

		void Awake()
		{
			_model = AmbitionApp.GetModel<PartyModel>();
			HostTelegraphBar.Subscribe(HandleTimer);
			AmbitionApp.Subscribe(PartyMessages.START_TURN, HandleStartTurn);
		}

		void OnDestroy()
		{
			HostTelegraphBar.Unsubscribe(HandleTimer);
			AmbitionApp.Unsubscribe(PartyMessages.START_TURN, HandleStartTurn);
		}

		private void HandleStartTurn()
		{
			float telegraphTime = 2.0f;
			HostTelegraphBar.CountUp(telegraphTime);
		}

		private void HandleTimer(TimerView timer)
		{
			if (timer.Complete)
			{
				AmbitionApp.SendMessage<GuestVO>(PartyMessages.HOST_REMARK, Host);
			}
		}

		private void HandleStartTimer()
		{
			// TODO: Clunky as hell. NEED a buff system.
			InventoryModel imod = AmbitionApp.GetModel<InventoryModel>();
			ItemVO accessory;
			float t = _model.TurnTime;
			if (imod.Equipped.TryGetValue(ItemConsts.ACCESSORY, out accessory) && accessory.Name == "Fan")
			{
				t *= 1.5f;
			}
			HostTelegraphBar.CountUp(t);
		}
	}
}
