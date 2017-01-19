using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalTabController : MonoBehaviour {
    public GameObject factionsTab;
    public GameObject factionsButton;
    public GameObject notablesTab;
    public GameObject notablesButton;
    public GameObject enemiesTab;
    public GameObject enemiesButton;

    // Use this for initialization
    void Start()
    {
        FactionsSelected();
    }

    public void FactionsSelected()
    {
        factionsTab.transform.SetAsLastSibling();
        factionsButton.GetComponent<Image>().color = Color.black;
        notablesButton.GetComponent<Image>().color = Color.white;
        enemiesButton.GetComponent<Image>().color = Color.white;
    }

    public void NotablesSelected()
    {
        notablesTab.transform.SetAsLastSibling();
        notablesButton.GetComponent<Image>().color = Color.black;
        factionsButton.GetComponent<Image>().color = Color.white;
        enemiesButton.GetComponent<Image>().color = Color.white;
    }

    public void EnemiesSelected()
    {
        enemiesTab.transform.SetAsLastSibling();
        enemiesButton.GetComponent<Image>().color = Color.black;
        factionsButton.GetComponent<Image>().color = Color.white;
        notablesButton.GetComponent<Image>().color = Color.white;
    }
}
