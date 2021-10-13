using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;
namespace Ambition
{
    public class SavedGameListItem : MonoBehaviour
    {
        public Text SaveText;
        public string Data;
        public string Snapshot;
        public GameObject DeleteBtn;

        public void SetData(string[] save)
        {
            SaveText.text = save[0];
            Snapshot = save[1];
            Data = save[save.Length - 1];
        }

        private bool _selected = false;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                DeleteBtn.SetActive(value);
            }
        }
    }
}
