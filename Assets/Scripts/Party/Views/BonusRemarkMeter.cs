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

		void Start ()
		{
			_model = _models.GetModel<PartyModel>();
			_meter.value = 0f;
			Label.text = "New Remark In: " + _model.FreeRemarkCounter.ToString();
		}

		void Awake()
		{
			_messages.Subscribe<int>(PartyConstants.TURN, HandleTurn);
			_meter = GetComponent<Slider>();
		}

		void OnDestroy()
		{
			_messages.Unsubscribe<int>(PartyConstants.TURN, HandleTurn);
			StopAllCoroutines();
		}

		private void HandleTurn(int turn)
		{
			int total = _model.FreeRemarkCounter;
			int turnsLeft = total - (turn%total);
			float fillAmount = (float)(turn%total)/(float)(total-1);
			StopAllCoroutines();
			Label.text = "New Remark In: " + turnsLeft.ToString();
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
