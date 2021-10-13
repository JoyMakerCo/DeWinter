using UnityEngine;
#if DEBUG
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Ambition;

namespace Core
{
    public class ConsoleView : MonoBehaviour
    {
        public CanvasGroup consoleCanvas;
		public Transform textContainer;
		public GameObject linePrefab;
		public InputField input;
		public ScrollRect scrollRect;

		const int maxTextLines = 2000;
		List<GameObject> lines;

		bool shown = false;

		void Awake()
		{
			shown = false;
			Hide();
			lines = new List<GameObject>();

			if (input != null)
			{
				input.onEndEdit.AddListener(HandleConsoleInput); 
			}
		}

		void OnDestroy()
		{
			AmbitionApp.Unsubscribe(GameMessages.TOGGLE_CONSOLE,ToggleConsole);

		}

		public void Configure()
		{
			AmbitionApp.Subscribe(GameMessages.TOGGLE_CONSOLE,ToggleConsole);
		}

		private Color GetColor( ConsoleStyle style )
		{
			switch (style)
			{
				case ConsoleStyle.Error:		return Color.red;
				case ConsoleStyle.Warning:		return Color.yellow;
				case ConsoleStyle.Log:			return Color.white;
				case ConsoleStyle.Input:		return Color.blue;
			}

			return Color.white;
		} 

		public void Add( string textLine, ConsoleStyle style = ConsoleStyle.Log )
		{
			var line = GameObject.Instantiate(linePrefab);

			var textComponent = line.GetComponent<Text>();
			textComponent.text = textLine;
			textComponent.color = GetColor(style);
			line.transform.SetParent(textContainer, false);

			lines.Add(line);

			// truncate the text if it gets crazy long
			var excess = lines.Count - maxTextLines;
			if (excess > 0) 
			{
				for (int i = 0; i < excess; i++)
				{
					GameObject.Destroy( lines[i] );
				}
				lines.RemoveRange(0, excess);
			}

			// scroll to bottom
			if (scrollRect != null)
			{
				StartCoroutine(ScrollToBottom());
			}
		}

		IEnumerator ScrollToBottom()
		{
			yield return new WaitForEndOfFrame();
			scrollRect.verticalNormalizedPosition = 0f;      
			scrollRect.horizontalNormalizedPosition = 0f;      
		}

		void ToggleConsole()
		{
			Debug.Log("ConsoleView.ToggleConsole");
			shown = !shown;
			if (shown)
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		public void Show()
		{	Debug.Log("ConsoleView.Show");

			consoleCanvas.alpha = 0.66f;
			consoleCanvas.interactable = true;
			consoleCanvas.blocksRaycasts = true;

			if (input != null)
			{
				input.ActivateInputField();
				input.Select();
			}
		}

		public void Hide()
		{
			Debug.Log("ConsoleView.Hide");
			consoleCanvas.alpha = 0.0f;
			consoleCanvas.interactable = false;
			consoleCanvas.blocksRaycasts = false;
		}

		void HandleConsoleInput( string inputText )
		{
			Debug.Log("Console input:  "+inputText);

			var console = AmbitionApp.GetModel<ConsoleModel>();
			if (console != null)
			{
				console.ParseInput(inputText);
			}
			else
			{
				Debug.LogWarning("Console model not available, can't do anything with input");
			}
		}
    }
}
#else
namespace Core
{
    public class ConsoleView : MonoBehaviour
    {
        void Awake() => Destroy(gameObject);
    }
}
#endif