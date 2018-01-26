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
	        textField = this.GetComponent<Text>();

	        //If the Servant has been introduced then their description doesn't show up
	        // TODO: What is this for???
//			textField.enabled = (AmbitionApp.GetModel<ServantModel>().Introduced.Count > 0);
	    }
	}
}
