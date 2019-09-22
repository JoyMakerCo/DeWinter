using System;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ambition
{
    public class DisableTimerCmd : ICommand
    {
        public void Execute()
        {
            GameObject canvas = Array.Find(SceneManager.GetActiveScene().GetRootGameObjects(), o => o.GetComponent<Canvas>() != null);
            if (canvas != null)
            {
                TurnTimerView[] timerViews = canvas.GetComponentsInChildren<TurnTimerView>();
                Array.ForEach(timerViews, v => v.enabled = false);
            }
        }
    }
}
