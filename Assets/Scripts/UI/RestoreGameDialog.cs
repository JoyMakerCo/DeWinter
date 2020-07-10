using System;
using Dialog;
using UnityEngine;
using Core;
namespace Ambition
{
    public class RestoreGameDialog : DialogView, IEscapeCloseDialog
    {
        public LoadGameView LoadView;
        public override void OnOpen() => LoadView.PopulateDialog();
    }
}
