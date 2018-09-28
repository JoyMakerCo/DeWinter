using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ambition
{
	public class IncidentDialogBox : MonoBehaviour, IPointerClickHandler
	{
		private Image _image;
		void Awake ()
		{
			_image = GetComponent<Image>();
			AmbitionApp.Subscribe<TransitionVO[]>(HandleTransitions);
		}
		
		void OnDestroy ()
		{
			AmbitionApp.Unsubscribe<TransitionVO[]>(HandleTransitions);
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			Next();
		}

		public void Next()
		{
			AmbitionApp.SendMessage<int>(IncidentMessages.INCIDENT_OPTION, 0);
            FMODUnity.RuntimeManager.PlayOneShot("event:/One Shot SFX/Mouse_click"); //Literally only ever plays this sound. It will never need to play anything else.
        }

		private void HandleTransitions(TransitionVO[] transitions)
		{
			_image.raycastTarget = transitions.Length <= 1;
		}
	}
}
