using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Ambition;

namespace Core
{
    public class ConsoleView : MonoBehaviour
    {
		public CanvasGroup consoleCanvas;
		public Transform textContainer;
		public GameObject linePrefab;
		public InputField input;

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

	

		public void Add( string textLine )
		{
			var line = GameObject.Instantiate(linePrefab);

			line.GetComponent<Text>().text = textLine;
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

			consoleCanvas.alpha = 0.5f;
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
