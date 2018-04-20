using System;
using System.Collections;
using Core;
using UnityEngine;
using UnityEngine.UI;

namespace Ambition
{
	public class BonusRemarkMeter : MonoBehaviour
	{
		private const float INTERP_TIME = 0.5f;

		private ModelSvc _models = App.Service<ModelSvc>();
		private MessageSvc _messages = App.Service<MessageSvc>();
		private PartyModel _model;
		private Slider _meter;

		public Text Label;

		void Awake()
		{
			_meter = GetComponent<Slider>();
			_model = _models.GetModel<PartyModel>();
		}

		void Start ()
		{
			_meter.value = 0f;
			Label.text = "New Remark In: " + _model.FreeRemarkCounter.ToString();
		}

		void OnEnable()
		{
			_messages.Subscribe<int>(PartyConstants.TURN, HandleTurn);
		}

		void OnDisable()
		{
			_messages.Unsubscribe<int>(PartyConstants.TURN, HandleTurn);
			StopAllCoroutines();
		}

		private void HandleTurn(int turn)
		{
			int total = _model.FreeRemarkCounter;
			turn = (turn-1)%total;
			float fillAmount = (float)(turn)/(float)(total-1);
			Label.text = "New Remark In: " + (total - turn).ToString();
			
			StopAllCoroutines();
			if (fillAmount < _meter.value) _meter.value = fillAmount;
			else StartCoroutine(InterpValue(fillAmount));
		}

		IEnumerator InterpValue(float value)
		{
			float v0 = _meter.value;
			for (float t = 0; t < INTERP_TIME; t+=Time.deltaTime)
			{
				_meter.value = (1f - t/INTERP_TIME)*v0 + t*value/INTERP_TIME;
				yield return null;
			}
			_meter.value = value;
		}
	}
}
