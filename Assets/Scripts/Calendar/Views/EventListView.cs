using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class EventListView : MonoBehaviour
	{
		public Transform ListContent;
		public Text[] Headers;

		public GameObject ListItemPrefab;

		private CalendarModel _calendar;
		private List<GameObject> _pool = new List<GameObject>();

		void OnEnable()
		{
			_calendar = AmbitionApp.GetModel<CalendarModel>();
			AmbitionApp.Subscribe<PartyVO>(PartyMessages.PARTY_UPDATE, HandleParty);
			UpdateParties();
		}

		void OnDisable()
		{
			AmbitionApp.Unsubscribe<PartyVO>(PartyMessages.PARTY_UPDATE, HandleParty);
		}

		private void HandleParty(PartyVO party)
		{
			if (party != null && party.Date >= _calendar.Today)
				UpdateParties();
		}

		private void UpdateParties()
		{
			List<PartyVO>[] types = new List<PartyVO>[]{
				new List<PartyVO>(),
				new List<PartyVO>(),
				new List<PartyVO>(),
				new List<PartyVO>()};

			Vector2 xform = Headers[0].rectTransform.anchoredPosition;
			List<PartyVO> parties;
			GameObject item;
			int itemCount=0;
			bool show;
			parties = _calendar.Parties.Where(p=>p.Key >= _calendar.Today).Select(p=>p.Value).ToArray().Aggregate(new List<PartyVO>(), (items,list)=>{items.AddRange(list); return items;});

			types[1] = parties.Where(p=>p.RSVP == 1).ToList();
			types[2] = parties.Where(p=>p.RSVP == -1).ToList();
			types[3] = parties.Where(p=>p.RSVP == 0).ToList();

			for (GameObject obj; _pool.Count < parties.Count; _pool.Add(obj))
			{
				obj = GameObject.Instantiate<GameObject>(ListItemPrefab, ListContent.transform);
			}

			while (_pool.Count > parties.Count)
			{
				GameObject.Destroy(_pool[0]);
				_pool.RemoveAt(0);
			}

			for(int i=0; i<types.Length; i++)
			{
				Headers[i].rectTransform.anchoredPosition = xform;
				show = types[i].Count > 0;
				Headers[i].enabled = show;
				if (show)
				{
					xform.y -= Headers[i].GetComponent<RectTransform>().rect.height;
					foreach(PartyVO party in types[i])
					{
						item = _pool[itemCount];
						item.GetComponent<EventListItemView>().Party = party;
						item.GetComponent<RectTransform>().anchoredPosition = xform;
						xform.y -= item.GetComponent<RectTransform>().rect.height;
						itemCount++;
					}
				}
			}
		}
	}
}
