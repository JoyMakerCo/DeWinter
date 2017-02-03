using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyInventory : MonoBehaviour {

    static EnemyInventory instance = null;

    public static List<Enemy> enemyInventory = new List<Enemy>();

    void Start () {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            print("Duplicate Enemy Inventory container self-destructing!");
        }
        else
        {
            instance = this;
            GameObject.DontDestroyOnLoad(gameObject);
        }
        //If the Player is of a sufficient Faction Level with the Military then all their Enemies in that Faction are surpressed
        if (GameData.factionList["Military"].PlayerReputationLevel() >= 9)
        {
            foreach (Enemy e in enemyInventory)
            {
                if(e.faction == GameData.factionList["Military"])
                {
                    RemoveEnemy(e);
                }
            }
        }
    }

    public void ClearInventory()
    {
        enemyInventory.Clear();
    }

    //Adds an Enemy to the Player's Enemy Inventory then scans the calendar going forward, adding the Enemy to appropriate Parties
    public static void AddEnemy(Enemy e)
    {
        if(e.faction == GameData.factionList["Military"] && GameData.factionList["Military"].PlayerReputationLevel() >= 9)
        {
            Debug.Log("These Enemies have been surpressed, due to the Player's Faction level with the Military");
        } else
        {
            enemyInventory.Add(e);
            AdditionPartyScan(e);
        }
    }

    //Adds the Enemy to future Parties, used in the Add Enemy Function
    static void AdditionPartyScan(Enemy e)
    {
        foreach(Month m in GameData.calendar.monthList)
        {
            if(m.monthNumber >= GameData.currentMonth)
            {
                foreach (Day d in m.dayList)
                {
                    if(m.monthNumber == GameData.currentMonth && d.linearDayValue >= GameData.currentDay)
                    {
                        if (d.party1.faction != null)
                        {
                            Party p = d.party1;
                            if (e.faction == GameData.factionList[p.faction])
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    p.AddEnemy(e);
                                }
                            }
                        }
                        if (d.party2.faction != null)
                        {
                            Party p = d.party2;
                            if (e.faction == GameData.factionList[p.faction])
                            {
                                if (Random.Range(0, 2) == 0)
                                {
                                    p.AddEnemy(e);
                                }
                            }
                        }
                    }                
                }
            }       
        } 
    }

    //Removes the Enemy from the Player's Inventory and from any future Parties
    public static void RemoveEnemy(Enemy e)
    {
        RemovalPartyScan(e);
        enemyInventory.Remove(e);
    }

    //Removes the Enemy from any future Parties, used in the Remove Enemies Function
    static void RemovalPartyScan(Enemy e)
    {
        foreach (Month m in GameData.calendar.monthList)
        {
            if (m.monthNumber >= GameData.currentMonth)
            {
                foreach (Day d in m.dayList)
                {
                    if (m.monthNumber == GameData.currentMonth && d.linearDayValue >= GameData.currentDay)
                    {
                        if (d.party1.faction != null)
                        {
                            Party p = d.party1;
                            if (p.enemyList.Contains(e))
                            {
                                p.RemoveEnemy(e);
                            }
                        }
                        if (d.party2.faction != null)
                        {
                            Party p = d.party2;
                            if (p.enemyList.Contains(e))
                            {
                                p.RemoveEnemy(e);
                            }
                        }
                    }
                }
            }
        }
    }
}
