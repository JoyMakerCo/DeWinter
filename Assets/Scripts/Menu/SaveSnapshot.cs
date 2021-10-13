using System;
using System.Collections;
using UnityEngine;
namespace Ambition
{
    public class SaveSnapshot : MonoBehaviour
    {
        private float SCALE = .25f;

        public void TakeSnapshot(Action<byte[]> callback) => StartCoroutine(Snap(callback));

        IEnumerator Snap(Action<byte[]> callback)
        {
            int culling = Camera.main.cullingMask;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("UI"));

            yield return new WaitForEndOfFrame(); // this also captures gui, remove if you don't wanna capture gui
            int width = Screen.width;
            int height = Screen.height;

            RenderTexture rt = new RenderTexture(width, height, 24, RenderTextureFormat.ARGB32);
            rt.useMipMap = false;
            rt.antiAliasing = 1;
            RenderTexture.active = rt;
            Camera.main.targetTexture = rt;

            Texture2D shot = new Texture2D(width, height, TextureFormat.ARGB32, false);
            Camera.main.Render();
            shot.ReadPixels(new Rect(0, 0, width, height), 0, 0, false);
            shot.Apply();

            Camera.main.targetTexture = null;
            RenderTexture.active = null;
            Destroy(rt);

            Camera.main.cullingMask = culling;

            callback(shot.EncodeToJPG());
        }

        private void OnApplicationQuit()
        {
            AmbitionApp.SendMessage(GameMessages.AUTOSAVE);
        }
    }
}
