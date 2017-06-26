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
			Ambition.Subscribe<string>(InventoryConsts.STYLE, UpdateStyle);
	        _text = gameObject.GetComponent<Text>();
			UpdateStyle(Ambition.GetModel<InventoryModel>().CurrentStyle);
	    }

	    void OnDestroy()
	    {
			Ambition.Unsubscribe<string>(InventoryConsts.STYLE, UpdateStyle);
	    }

		private void UpdateStyle(string style)
	    {
	        _text.text = style;
	    }
	}
}