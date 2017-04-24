using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DeWinter;

public class BoozeGlassTextController : MonoBehaviour {

    private Text _text;
    private PartyModel _model;

    // Use this for initialization
    void Awake ()
    {
    	_model = DeWinterApp.GetModel<PartyModel>();
        _text = this.GetComponent<Text>();
		DeWinterApp.Subscribe<int>(GameConsts.DRINK, HandleGlass);
	}

	void OnDestroy()
    {
		DeWinterApp.Subscribe<int>(GameConsts.DRINK, HandleGlass);
    }
	
	// Update is called once per frame
	void HandleGlass(int tox)
	{
        _text.text = "Booze Glass: " + tox.ToString() + "/" + _model.MaxDrinkAmount.ToString();
    }
}
