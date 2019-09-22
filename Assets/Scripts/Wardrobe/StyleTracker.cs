using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Ambition;

namespace Ambition
{
	public class StyleTracker : MonoBehaviour
	{
	    private Text _text;

	    void Awake()
	    {
			AmbitionApp.Subscribe<string>(ItemConsts.STYLE, UpdateStyle);
	        _text = gameObject.GetComponent<Text>();
			UpdateStyle(AmbitionApp.GetModel<InventoryModel>().Style);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<string>(ItemConsts.STYLE, UpdateStyle);
	    }

		private void UpdateStyle(string style)
	    {
	        _text.text = style;
	    }
	}
}