using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Ambition;

public class BoozeGlassTextController : MonoBehaviour {

    private Text _text;
    private PartyModel _model;

    // Use this for initialization
    void Awake ()
    {
    	_model = AmbitionApp.GetModel<PartyModel>();
        _text = this.GetComponent<Text>();
		AmbitionApp.Subscribe<int>(GameConsts.DRINK, HandleGlass);
	}

	void OnDestroy()
    {
		AmbitionApp.Subscribe<int>(GameConsts.DRINK, HandleGlass);
    }
	
	// Update is called once per frame
	void HandleGlass(int tox)
	{
        _text.text = "Booze Glass: " + tox.ToString() + "/" + _model.MaxDrinkAmount.ToString();
    }
}
