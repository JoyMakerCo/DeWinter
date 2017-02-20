using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyStatsTracker : MonoBehaviour {

    private Text enemyNameText;
    private Text enemyFlavorText;
    private Text enemyFactionText;

    //Enemy List
    public EnemyList enemyList;

    // Use this for initialization
    void Start () {
        //Text
        enemyNameText = this.transform.Find("EnemyNameText").GetComponent<Text>();
        enemyFlavorText = this.transform.Find("EnemyFlavorText").GetComponent<Text>();
        enemyFactionText = this.transform.Find("EnemyFactionText").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        if (enemyList.selectedEnemy != null)
        {
            //Text
            enemyNameText.text = enemyList.selectedEnemy.Name;
            enemyFlavorText.text = enemyList.selectedEnemy.FlavorText();
            enemyFactionText.text = enemyList.selectedEnemy.Faction;
        }
        else
        {
            enemyNameText.text = "";
            enemyFlavorText.text = "";
            enemyFactionText.text = "";
        }
    }
}
