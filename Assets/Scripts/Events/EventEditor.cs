using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  
using UnityEditor;  
using UnityEditorInternal;

namespace Ambition
{
	public interface IGraphComponent
	{
		void OnGUI();
		bool Intersect(Vector2 MousePosition);

		string InspectorTitle { get; }
		Vector2 InspectorSize { get; }
		void OnInspector();
	}

	public class MomentVO : IGraphComponent
	{
		private const float NODE_WIDTH = 200f;
		private const float NODE_HEIGHT = 50f;
		private const float INSPECTOR_WIDTH = 400f;
		private const float INSPECTOR_HEIGHT = 100f;

		public Dictionary<MomentVO, int> Links;
		public string Text;

		private Rect _rect=new Rect(0,0,NODE_WIDTH, NODE_HEIGHT);
		private Rect _textRect=new Rect(5,20,INSPECTOR_WIDTH-10, INSPECTOR_HEIGHT-25);
		public Rect Rect
		{
			get { return _rect; }
		}
		public Vector2 Position
		{
			set { _rect.center = value; }
			get { return _rect.center; }
		}

		public MomentVO(string text)
		{
			Text = text;
		}

		public string InspectorTitle
		{
			get { return "Editing Moment…"; }
		}

		public Vector2 InspectorSize
		{
			get { return new Vector2(INSPECTOR_WIDTH, INSPECTOR_HEIGHT); }
		}

		public void OnInspector()
		{
			Text = GUI.TextArea(
				_textRect,
				Text);
		}

		public bool Intersect(Vector2 MousePosition)
		{
			return _rect.Contains(MousePosition);
		}

		public void OnGUI()
		{
			GUI.Box(_rect, Text);
		}
	}

	public class LinkVO : IGraphComponent
	{
		private const float INSPECTOR_WIDTH = 400f;
		private const float INSPECTOR_HEIGHT = 50;

		public string Text;
		public int Probability;

		public MomentVO Start;
		public MomentVO End;

		protected Vector2 _connectionStart;
		protected Vector2 _connectionEnd;

		public LinkVO(MomentVO start, MomentVO end)
		{
			Start = start;
			End = end;
			Redraw();
		}

		public void OnGUI()
		{
			Handles.color = Color.black;
			Handles.DrawLine(_connectionStart, _connectionEnd);
		}
	
		public bool Intersect(Vector2 MousePosition)
		{
			return false;
		}

		public string InspectorTitle
		{
			get { return "Editing Link"; }
		}

		public Vector2 InspectorSize
		{
			get { return new Vector2(INSPECTOR_WIDTH, INSPECTOR_HEIGHT); }
		}

		public void OnInspector()
		{
			Text=GUI.TextField(
				new Rect(5, 20, INSPECTOR_WIDTH-10, INSPECTOR_HEIGHT-25),
				Text);
		}

		public void Redraw()
		{
			if (Start != null && End != null)
			{
				// TODO: Draw to nearest borders instead of centers
				// TODO: Add arrow heads
				_connectionStart = Start.Rect.center;
				_connectionEnd = End.Rect.center;
			}
		}
	}

	public class EventEditor : EditorWindow
	{
		private const int NUM_VISIBLE_LINES = 4;
		private const float CONNECT_BTN_WIDTH = 10f;

		private EventCollection _collection;
		private EventConfig _config;
		private SerializedObject _collectionObj;
		private SerializedProperty _configProp;
		private bool _dragging;
		private Vector2 _scroll;
		private Vector2 _mousePos;
		private bool _dirty=false;
		private IGraphComponent _selected;

		private Rect _editorWindowRect;
		private bool _isEditorOpen=false;
		private bool _isMenuOpen=false;
		private bool _closing = false;

		private List<MomentVO> _moments = new List<MomentVO>();
		private List<LinkVO> _links = new List<LinkVO>();

	    public static void Show(SerializedObject obj)
	    {
			EventEditor editor = EditorWindow.GetWindow<EventEditor>();
	    	if (obj != null && obj.targetObject is EventCollection)
	    	{
				editor.SetCollection(obj);
			}
			else editor.OnDestroy();
		}

		public static void Show(EventCollection collection)
	    {
			EventEditor editor = EditorWindow.GetWindow<EventEditor>();
	    	if (collection != null)
	    	{
				editor.SetCollection(new SerializedObject(collection));
			}
			else editor.OnDestroy();
	    }

	    void OnSelectionChange()
	    {
	    	OnDestroy();
	    }

	    void OnDestroy()
	    {
	    	_collection = null;
			_config = null;
			_collectionObj = null;
			_configProp = null;
		}

	    public void OnGUI()
	    {
	    	if (_collectionObj != null && _config != null)
	    	{
				_scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scroll, new Rect(0, 0, 1000, 1000));
				_collectionObj.Update();
				_links.ForEach(l => l.OnGUI());
				_moments.ForEach(m => m.OnGUI());
				if (_isEditorOpen)
				{
					_mousePos = Event.current.mousePosition;
					if (Event.current.isMouse && !_editorWindowRect.Contains(_mousePos))
						_isEditorOpen = false;
					else if (Event.current.isKey && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.Escape))
						_isEditorOpen = false;
					if (!_isEditorOpen) _closing = _dirty = true;
				}
				if (!_isEditorOpen)
				{
					switch(Event.current.type)
					{
						case EventType.MouseDown:
							_mousePos = Event.current.mousePosition;
							if (Event.current.button == 0)
							{
								_selected = _moments.Find(c=>c.Intersect(_mousePos));
								if (_selected == null) _links.Find(l=>l.Intersect(_mousePos));
								_dragging = _selected != null;
							}
							break;
						case EventType.MouseDrag:
							if (_dragging && !_isEditorOpen)
							{
								_isEditorOpen = false;
								((MomentVO)_selected).Position += Event.current.mousePosition - _mousePos;
								_dirty = true;
								_mousePos = Event.current.mousePosition;
							}
							break;
						case EventType.MouseUp:
							_dragging = false;
							_mousePos = Event.current.mousePosition;

							if (!_closing && _selected != null && _selected.Intersect(_mousePos))
							{
								ShowEditor(_mousePos);
							}

							if (!_isEditorOpen)
							{
								MomentVO moment = _moments.Find(m => m.Intersect(_mousePos));
								ShowMenu(moment, Event.current.button);
							}
							_closing = false;
							break;
					}
				}
				if (_isEditorOpen && _selected != null)
				{
					BeginWindows();
					_editorWindowRect = GUI.Window(0, _editorWindowRect, DrawEditor, _selected.InspectorTitle);
					GUI.FocusWindow(0);
					EndWindows();
				}
				if (_dirty)
				{
					if (_selected != null)
					{
						_links.FindAll(l=>l.Start == _selected || l.End == _selected).ForEach(l=>l.Redraw());
					}
					Reserialize();
					Repaint();
					_dirty = false;
				}
				_collectionObj.ApplyModifiedProperties();
				GUI.EndScrollView();
			}
	    }

		private void SetCollection(SerializedObject obj)
		{
			if (_collectionObj == null || _collectionObj.targetObject != obj.targetObject)
			{
				_collectionObj = obj;
				_collection = (EventCollection)(_collectionObj.targetObject);
				_configProp = obj.FindProperty("_selectedConfig");
			}
		 	_config = _collection._selectedConfig;
			if (_config != null)
			{
				int count = _config.Moments == null ? 0 : _config.Moments.Length;
				MomentVO moment;
				LinkVO link;
				titleContent.text = _config.Name;
				_moments = new List<MomentVO>(count);
				for (int i=0; i<count; i++)
				{
					moment = new MomentVO(_config.Moments[i]);
					moment.Position = (i < _config.Positions.Length) ? _config.Positions[i] : Vector2.zero;
					_moments.Add(moment);
				}
				_links = new List<LinkVO>();
				for (int i=_config.Links.Length-1; i>=0; i--)
				{
					link = new LinkVO(_moments[_config.Links[i].Index], _moments[_config.Links[i].Target]);
					link.Text = _config.Links[i].Text;
					_links.Add(link);
				}
			}
	    }

	    private void Reserialize()
	    {
			SerializedProperty moments = _configProp.FindPropertyRelative("Moments");
			SerializedProperty positions = _configProp.FindPropertyRelative("Positions");
			SerializedProperty links = _configProp.FindPropertyRelative("Links");
			int count = _moments.Count;
			List<LinkVO> linkvos;
			LinkVO link;
			SerializedProperty sLink;
			moments.arraySize = count;
			positions.arraySize = count;
			links.arraySize = _links.Count;
			for (int i=count-1; i>=0; i--)
			{
				moments.GetArrayElementAtIndex(i).stringValue = _moments[i].Text;
				positions.GetArrayElementAtIndex(i).vector2Value = _moments[i].Position;
			}
			for (int i=_links.Count-1; i>=0; i--)
			{
				link = _links[i];
				sLink = links.GetArrayElementAtIndex(i);
				sLink.FindPropertyRelative("Index").intValue = _moments.IndexOf(link.Start);
				sLink.FindPropertyRelative("Target").intValue = _moments.IndexOf(link.End);
				sLink.FindPropertyRelative("Text").stringValue = link.Text;
			}
	    }

	    private void ShowEditor(Vector2 mousePos)
	    {
			_isEditorOpen = true;
			_isMenuOpen = false;
			_editorWindowRect.position = mousePos;
			_editorWindowRect.size = _selected.InspectorSize;
			_dirty = true;
	    }

		private void ShowMenu(IGraphComponent component, int whichButton)
		{
			_isEditorOpen = false;
			if (!_isMenuOpen)
			{
				GenericMenu menu = new GenericMenu();
				if (component == null)
				{
					if (whichButton == 0 || _selected == null)
					{
						menu.AddItem(new GUIContent("Create New Moment"), false, CreateNewMoment);
					}
					else
					{
						menu.AddItem(new GUIContent("Create And Link New Moment"), false, LinkNewMoment);
					}
				}
				else if (whichButton == 1 && component != _selected)
				{
					menu.AddItem(new GUIContent("Link to Moment"), false, LinkMoment, component);
				}
				menu.ShowAsContext();
			}
			_isMenuOpen = !_isMenuOpen;
		}

		private void DrawEditor(int windowID)
		{
			_selected.OnInspector();
			GUI.DragWindow();
		}

		void CreateNewMoment()
		{
			MomentVO moment = new MomentVO("New Moment");
			moment.Position = _mousePos;
			_moments.Add(moment);
			_selected = moment;
			ShowEditor(_mousePos);
		}

		void LinkNewMoment()
		{
			MomentVO moment = new MomentVO("New Moment");
			moment.Position = _mousePos;
			_moments.Add(moment);
			CreateLink(_selected as MomentVO, moment);
		}

		void LinkMoment(object obj)
		{
			MomentVO moment = obj as MomentVO;
			if (moment is MomentVO) CreateLink((MomentVO)_selected, moment);
		}

		private void CreateLink(MomentVO start, MomentVO end)
		{
			if (start != null && end != null)
			{
				LinkVO link = new LinkVO(start, end);
				link.Start = start;
				link.End = end;
				_links.Add(link);
				_selected = link;
				ShowEditor((link.Start.Rect.center+link.End.Rect.center)*.5f);
			}
	    }
	}
}
  