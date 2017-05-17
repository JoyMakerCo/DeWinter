using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Ambition
{
	public class PartyTurnTextTracker : MonoBehaviour
	{
	    private Text _text;

		// Use this for initialization
		void Start ()
		{
	        _text = this.GetComponent<Text>();
	        AmbitionApp.Subscribe<int>(PartyConstants.TURNSLEFT, HandleTurnsLeft);
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe<int>(PartyConstants.TURNSLEFT, HandleTurnsLeft);
		}
		
		// Update is called once per frame
		private void HandleTurnsLeft (int turns)
		{
	        _text.text = "Turns: " + turns + "/" + AmbitionApp.GetModel<PartyModel>().Party.Turns;
			_text.color = turns > 0 ? Color.white : Color.red;
		}
	}
}