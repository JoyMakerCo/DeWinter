using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalTabController : MonoBehaviour {
    public GameObject personalTab;
    public GameObject personalButton;
    public GameObject factionsTab;
    public GameObject factionsButton;
    public GameObject notablesTab;
    public GameObject notablesButton;
    public GameObject enemiesTab;
    public GameObject enemiesButton;
    public Text enemiesButtonText;

    public GeneralReputationBenefitsListController benefitsController;

    // Use this for initialization
    void Start()
    {
        PersonalSelected();
    }

    public void PersonalSelected()
    {
        benefitsController.DisplayBenefits(); //Just making sure the the Benefits are properly tallied up and displayed
        personalTab.transform.SetAsLastSibling();
        personalButton.GetComponent<Image>().color = Color.black;
        factionsButton.GetComponent<Image>().color = Color.white;
        notablesButton.GetComponent<Image>().color = Color.white;
        if (EnemyInventory.enemyInventoryUnlocked) // Can only see this tab if the Player actually has Enemies
        {
            enemiesButton.GetComponent<Image>().color = Color.white;
            enemiesButtonText.color = Color.white;
        } else
        {
            enemiesButton.GetComponent<Image>().color = Color.clear;
            enemiesButtonText.color = Color.clear;
        }
    }

    public void FactionsSelected()
    {
        factionsTab.transform.SetAsLastSibling();
        factionsButton.GetComponent<Image>().color = Color.black;
        personalButton.GetComponent<Image>().color = Color.white;
        notablesButton.GetComponent<Image>().color = Color.white;
        if (EnemyInventory.enemyInventoryUnlocked) // Can only see this tab if the Player actually has Enemies
        {
            enemiesButton.GetComponent<Image>().color = Color.white;
            enemiesButtonText.color = Color.white;
        }
        else
        {
            enemiesButton.GetComponent<Image>().color = Color.clear;
            enemiesButtonText.color = Color.clear;
        }
    }

    public void NotablesSelected()
    {
        notablesTab.transform.SetAsLastSibling();
        notablesButton.GetComponent<Image>().color = Color.black;
        personalButton.GetComponent<Image>().color = Color.white;
        factionsButton.GetComponent<Image>().color = Color.white;
        if (EnemyInventory.enemyInventoryUnlocked) // Can only see this tab if the Player actually has Enemies
        {
            enemiesButton.GetComponent<Image>().color = Color.white;
            enemiesButtonText.color = Color.white;
        }
        else
        {
            enemiesButton.GetComponent<Image>().color = Color.clear;
            enemiesButtonText.color = Color.clear;
        }
    }

    public void EnemiesSelected()
    {
        if (EnemyInventory.enemyInventoryUnlocked) // Can only open this tab if the Player actually has Enemies
        {
            enemiesTab.transform.SetAsLastSibling();
            enemiesButton.GetComponent<Image>().color = Color.black;
            personalButton.GetComponent<Image>().color = Color.white;
            factionsButton.GetComponent<Image>().color = Color.white;
            notablesButton.GetComponent<Image>().color = Color.white;
        }    
    }
}
