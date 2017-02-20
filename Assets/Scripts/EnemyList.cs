using UnityEngine;
using System.Collections;

public class EnemyList : MonoBehaviour {

    public Enemy selectedEnemy;
    public GameObject enemyButtonPrefab;

    void Start()
    {
        GenerateEnemyButtons();
        selectedEnemy = null; // So nothing is selected at the start
    }

    public void GenerateEnemyButtons()
    {
        foreach (Enemy e in EnemyInventory.enemyInventory)
        {
            GameObject button = GameObject.Instantiate(enemyButtonPrefab);
            EnemyButton buttonStats = button.GetComponent<EnemyButton>();
            buttonStats.enemy = e;
            button.transform.SetParent(this.transform, false);
            Debug.Log("Enemy Button: " + e.Name + " has been made!");
        }
    }

    public void ClearInventoryButtons()
    {
        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
