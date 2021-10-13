using UnityEngine;
using UnityEngine.UI;
namespace Ambition
{
    public interface ISubmitHandler
    {
        void Submit();
        void Cancel();
    }

    public interface IButtonInputHandler
    {
        void HandleInput(string id, bool holding);
    }

    public interface IDirectionInputHandler
    {
        void HandleInput(int x, int y);
    }

    public interface IAnalogInputHandler
    {
        void HandleInput(Vector2[] sticks);
    }

    public interface IScrollInputHandler
    {
        ScrollRect[] GetScrollRects();
    }
}
