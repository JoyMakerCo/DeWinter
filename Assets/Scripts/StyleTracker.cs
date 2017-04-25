using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using DeWinter;

namespace DeWinter
{
	public class StyleTracker : MonoBehaviour
	{
	    private Text _text;

	    void Awake()
	    {
			DeWinterApp.Subscribe<string>(InventoryConsts.STYLE, UpdateStyle);
	        _text = gameObject.GetComponent<Text>();
			UpdateStyle(DeWinterApp.GetModel<InventoryModel>().CurrentStyle);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<string>(InventoryConsts.STYLE, UpdateStyle);
	    }

		private void UpdateStyle(string style)
	    {
	        _text.text = style;
	    }
	}
}