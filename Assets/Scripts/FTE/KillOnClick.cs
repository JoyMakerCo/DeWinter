using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
	// Useful for Tutorials, this component Destroys the attached gameObject when any of the target objects are clicked
	public class KillOnClick : MonoBehaviour
	{
		public GameObject[] Targets; // Clicking on any of these targets dismisses the tutorial step

		private EventTrigger.Entry _event;
		private bool _enabled=false;

		void OnEnable()
		{
			if (!_enabled)
			{
				EventTrigger trigger;
				_event = new EventTrigger.Entry();
				_event.eventID = EventTriggerType.PointerDown;
				_event.callback.AddListener((data) => { EndStepDelegate((PointerEventData)data); });

				foreach (GameObject s in Targets)
				{
					if (s != null)
					{
						trigger = s.GetComponent<EventTrigger>();
						if (trigger == null) trigger = s.AddComponent<EventTrigger>();
						trigger.triggers.Add(_event);
					}
				}
				_enabled = true;
			}
		}

		private void EndStepDelegate(PointerEventData data)
		{
			EventTrigger trigger;
			EventTrigger.Entry e = new EventTrigger.Entry();
			foreach (GameObject s in Targets)
			{
				if (s != null)
				{
					trigger = s.GetComponent<EventTrigger>();
					Debug.Assert(trigger != null, "Somehow, a trigger is null");
					trigger.triggers.Remove(_event);
				}
			}
			Destroy(this.gameObject);
		}
	}
}
