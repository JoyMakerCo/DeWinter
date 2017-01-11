using UnityEngine;
using System.Collections;

public class ExitGamePopUpModalController : MonoBehaviour {

    LevelManager levelManager;

	// Use this for initialization
	void Start () {
        levelManager = FindObjectOfType<LevelManager>();
	}
	
	public void QuitGame()
    {
        levelManager.QuitRequest();
    }
}
