using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class ServantTextController : MonoBehaviour {

	    private Text textField;
	    public string servantType;

	    void Start ()
	    {
	    	List<ServantVO> servants;
	        textField = this.GetComponent<Text>();

	        //If the Servant has been introduced then their description doesn't show up
			if (AmbitionApp.GetModel<ServantModel>().Introduced.TryGetValue(servantType, out servants) && servants.Count > 0)
	        {
	            textField.color = Color.white;
	        } else
	        {
	            textField.color = Color.clear;
	        }
	    }
	}
}