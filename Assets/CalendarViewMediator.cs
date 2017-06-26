using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ambition
{
	public class CalendarViewMediator : MonoBehaviour
	{
		private CalendarModel _model;

		void Awake()
		{
			_model = DeWinterApp.GetModel<CalendarModel>();
		}

		// Use this for initialization
		void Start ()
		{
			// Trigger an event.
			_model.Today = _model.Today;
		}
	}
}