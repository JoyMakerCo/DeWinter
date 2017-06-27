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
			AmbitionApp.Subscribe<string>(InventoryConsts.STYLE, UpdateStyle);
	        _text = gameObject.GetComponent<Text>();
			UpdateStyle(AmbitionApp.GetModel<InventoryModel>().CurrentStyle);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<string>(InventoryConsts.STYLE, UpdateStyle);
	    }

		private void UpdateStyle(string style)
	    {
	        _text.text = style;
	    }
	}
}