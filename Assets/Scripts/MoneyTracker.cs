using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace DeWinter
{
	public class MoneyTracker : MonoBehaviour
	{
	    private Text myText;
	    public GameObject screenFader;

	    // Use this for initialization
	    void Start()
	    {
	        myText = GetComponent<Text>();
			DeWinterApp.Subscribe<int>(GameConsts.LIVRE, HandleBalanceUpdate);
	    }

	    void OnDestroy()
	    {
			DeWinterApp.Unsubscribe<int>(GameConsts.LIVRE, HandleBalanceUpdate);
	    }

		private void HandleBalanceUpdate(int livre)
		{
			myText.text = livre.ToString("£" + "#,##0");
		}
	}
}