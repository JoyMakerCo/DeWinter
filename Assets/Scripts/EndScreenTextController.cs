using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace DeWinter
{
	public class EndScreenTextController : MonoBehaviour
	{
	    public Text titleText;
	    public Text bodyText;

	    void Start()
	    {
			DeWinterApp.SendCommand<CheckVictoryConditionCmd, EndScreenTextController>(this);
	    }
	}
}