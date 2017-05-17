using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Ambition
{
	public class MoneyTracker : MonoBehaviour
	{
	    private Text myText;
	    public GameObject screenFader;

	    // Use this for initialization
	    void Start()
	    {
	        myText = GetComponent<Text>();
			AmbitionApp.Subscribe<int>(GameConsts.LIVRE, HandleBalanceUpdate);
	    }

	    void OnDestroy()
	    {
			AmbitionApp.Unsubscribe<int>(GameConsts.LIVRE, HandleBalanceUpdate);
	    }

		private void HandleBalanceUpdate(int livre)
		{
			myText.text = livre.ToString("£" + "#,##0");
		}
	}
}