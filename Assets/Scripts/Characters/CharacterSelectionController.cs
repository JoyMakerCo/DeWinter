using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Core;

namespace Ambition
{
    public class CharacterSelectionController : Dialog.DialogView, ISubmitHandler
    {
        public PlayerConfig[] Characters;

        private PlayerConfig _selected;

        public override void OnOpen()
        {
            _selected = Characters[0];
            Submit();
        }

        public void Cancel() => Close();
        public void Submit()
        {
            App.Service<CommandSvc>().Execute<NewGameCmd, string>(_selected.name);
            Close();
        }
    }
}
