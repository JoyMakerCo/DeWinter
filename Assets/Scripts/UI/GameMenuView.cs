using System;
using Core;

namespace Ambition
{
    public class GameMenuView : Dialog.DialogView, IEscapeCloseDialog
    {
        public void Save() => AmbitionApp.Save();
    }
}
