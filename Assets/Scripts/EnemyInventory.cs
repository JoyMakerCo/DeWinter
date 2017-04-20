using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using DeWinter;
using System.Linq;

public static class EnemyInventory
{
    // TODO: This needs a model.
    public static List<Enemy> enemyInventory = new List<Enemy>();

    public static void ClearInventory()
    {
        enemyInventory.Clear();
    }

    //Adds an Enemy to the Player's Enemy Inventory then scans the calendar going forward, adding the Enemy to appropriate Parties
    public static void AddEnemy(Enemy e)
    {
		FactionModel fmod = DeWinterApp.GetModel<FactionModel>();

        if(e.Faction == "Military" && fmod["Military"].ReputationLevel >= 9)
        {
            Debug.Log("These Enemies have been surpressed, due to the Player's Faction level with the Military");
        } else
        {
            enemyInventory.Add(e);
            AdditionPartyScan(e);
        }
    }

    //Adds the Enemy to future Parties, used in the Add Enemy Function
// TODO: This really needs to be determined at the start of the party.
    private static void AdditionPartyScan(Enemy e)
    {
    	CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
		List<DateTime> dates = cmod.Parties.Keys.ToList().FindAll(d => d > DeWinterApp.GetModel<CalendarModel>().Today);
    	System.Random rnd = new System.Random();
    	foreach(DateTime date in dates)
    	{
			for (int i=cmod.Parties[date].Count-1; i>=0; i--)
			{
				if (e.Faction == cmod.Parties[date][i].faction && rnd.Next(2) == 0)
					cmod.Parties[date][i].enemyList.Add(e);
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
    // TODO: Populate enemies when parties start, so that this method doesn't have to exist
    private static void RemovalPartyScan(Enemy e)
    {
		CalendarModel cmod = DeWinterApp.GetModel<CalendarModel>();
		List<DateTime> dates = cmod.Parties.Keys.ToList().FindAll(d => d >= DeWinterApp.GetModel<CalendarModel>().Today);
    	foreach (DateTime d in dates)
    	{
    		for (int i=cmod.Parties[d].Count-1; i>=0; i--)
    		{
				cmod.Parties[d][i].enemyList.Remove(e);
    		}
    	}
    }
}