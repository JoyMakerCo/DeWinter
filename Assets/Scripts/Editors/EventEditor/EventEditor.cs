using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  
using UnityEditor;

namespace Ambition
{
	public interface IGraphComponent
	{
		void OnGUI(bool selected);
		bool Intersect(Vector2 MousePosition);
	}

	public class EventEditor : EditorWindow
	{
		private const int NUM_VISIBLE_LINES = 4;
		private const float CONNECT_BTN_WIDTH = 10f;

		private static EventEditor _editor;

		private SerializedObject _collection;
		private SerializedProperty _config;
		private Vector2 _scroll;
		private Rect _scrollRect;
		private Vector2 _mousePos;		
		private bool _dragging=false;
		private bool _dirty=false;
		private IGraphComponent _selected;

		private List<EventNodeVO> _nodes = new List<EventNodeVO>();
		private List<EventLinkVO> _links = new List<EventLinkVO>();

	    public static void Show(SerializedObject obj)
	    {
			_editor = EditorWindow.GetWindow<EventEditor>();
	    	if (obj != null && obj.targetObject is EventCollection)
	    	{
				_editor.SetCollection(obj);
			}
			else _editor.OnDestroy();
		}

		public static void Show(EventCollection collection)
	    {
			_editor = EditorWindow.GetWindow<EventEditor>();
	    	if (collection != null)
	    	{
				_editor.SetCollection(new SerializedObject(collection));
			}
			else _editor.OnDestroy();
	    }

	    public static void InspectorUpdated()
	    {
	    	if (_editor != null && _editor._collection != null)
				_editor.SetCollection(_editor._collection);
	    }

	    void OnSelectionChange()
	    {
	    	OnDestroy();
	    }

	    void OnDestroy()
	    {
	    	_editor = null;
			_collection = null;
			_config = null;
			titleContent.text = "Event Editor";
			Repaint();
		}

	    public void OnGUI()
	    {
	    	if (_collection != null && _config != null)
	    	{
				_collection.Update();
				_scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scroll, _scrollRect);
				switch(Event.current.type)
				{
					case EventType.KeyDown:
						if (Event.current.modifiers == EventModifiers.None)
						{
							EditorWindow w = GetWindow<EditorWindow>("UnityEditor.InspectorWindow");
							if (w != null) w.Focus();
						}
						break;
					case EventType.MouseDown:
						_mousePos = Event.current.mousePosition;
						IGraphComponent comp = _nodes.Find(m=>m.Intersect(_mousePos));
						if (comp == null) comp = _links.Find(l=>l.Intersect(_mousePos));
						else _dragging = Event.current.button == 0;
						if (Event.current.button == 1) ShowMenu(comp);
						Selected = comp;
						break;

					case EventType.MouseDrag:
						if (_dragging)
						{
							((EventNodeVO)Selected).Position += Event.current.mousePosition - _mousePos;
							_dirty = true;
							_mousePos = Event.current.mousePosition;
						}
						break;

					case EventType.MouseUp:
						_dragging = false;
						_mousePos = Event.current.mousePosition;
						break;
				}
				if (_dirty)
				{
					if (_dragging)
					{
						_links.FindAll(l=>l.Start == Selected || l.End == Selected).ForEach(l=>l.Redraw());
						AdjustScrollRect(((EventNodeVO)Selected).Rect);
					}
					_dirty = false;
					Reserialize();
					Repaint();
				}
				_links.ForEach(l => l.OnGUI(l == Selected));
				_nodes.ForEach(m => m.OnGUI(m == Selected));
				_collection.ApplyModifiedProperties();
				GUI.EndScrollView();
			}
	    }

	    private IGraphComponent Selected
	    {
	    	get { return _selected; }
	    	set {
				bool isMoment = value is EventNodeVO;
				int index = (value == null ? -1 : isMoment ? _nodes.IndexOf(value as EventNodeVO) : _links.IndexOf(value as EventLinkVO));
	    		_collection.FindProperty(EventCollection.SELECTED_COMPONENT).intValue = index;
				_collection.FindProperty(EventCollection.IS_MOMENT).boolValue = isMoment;
	    		_selected = value;
	    	}
	    }

		private void DeleteComponent(object component)
		{
			Selected = null;
			_dirty = true;
			if (!_links.Remove(component as EventLinkVO))
			{
				_links.RemoveAll(l=>l.Start == component || l.End == component);
				_nodes.Remove((EventNodeVO)component);
			}
		}

		private void SetCollection(SerializedObject obj)
		{
			EventCollection collection;
			EventVO config;
			int index;

			_collection = obj;
			collection = (EventCollection)(_collection.targetObject);
			index = collection._selectedEvent;
			config = collection.Events.Length > index ? collection.Events[index] : null;
			if (config != null)
			{
				int count = config.Moments == null ? 0 : config.Moments.Length;
				EventNodeVO node;
				EventLinkVO link;

				_config = obj.FindProperty("Events").GetArrayElementAtIndex(index);

				titleContent.text = "Event Editor - " + config.Name;
				_nodes = new List<EventNodeVO>();
				_scrollRect = new Rect();
				for (int i=0; i<count; i++)
				{
					node = new EventNodeVO(config.Moments[i].Text);
					node.Position = (i < config.Positions.Length) ? config.Positions[i] : Vector2.zero;
					_nodes.Add(node);
					AdjustScrollRect(node.Rect);
				}
				_links = new List<EventLinkVO>();
				count = config.Links.Length;
				for (int i=0; i<count; i++)
				{
					link = new EventLinkVO(_nodes[config.Links[i].Index], _nodes[config.Links[i].Target]);
					link.Text = config.Links[i].Text;
					_links.Add(link);
				}
			}
			Repaint();
	    }

	    private void Reserialize()
	    {
			SerializedProperty moments = _config.FindPropertyRelative("Moments");
			SerializedProperty positions = _config.FindPropertyRelative("Positions");
			SerializedProperty links = _config.FindPropertyRelative("Links");
			int count = _nodes.Count;
			EventLinkVO link;
			SerializedProperty sLink;
			moments.arraySize = count;
			positions.arraySize = count;
			links.arraySize = _links.Count;
			for (int i=count-1; i>=0; i--)
			{
				moments.GetArrayElementAtIndex(i).FindPropertyRelative("Text").stringValue = _nodes[i].Text;
				positions.GetArrayElementAtIndex(i).vector2Value = _nodes[i].Position;
			}
			for (int i=_links.Count-1; i>=0; i--)
			{
				link = _links[i];
				sLink = links.GetArrayElementAtIndex(i);
				sLink.FindPropertyRelative("Index").intValue = _nodes.IndexOf(link.Start);
				sLink.FindPropertyRelative("Target").intValue = _nodes.IndexOf(link.End);
				sLink.FindPropertyRelative("Text").stringValue = link.Text;
			}
	    }

		private void ShowMenu(IGraphComponent component)
		{
			GenericMenu menu = new GenericMenu();
			if (component == null)
			{
				menu.AddItem(new GUIContent("Create New Moment"), false, CreateNewNode, null);
				if (Selected != null)
					menu.AddItem(new GUIContent("Create And Link New Moment"), false, CreateNewNode, Selected);
			}
			else if (component is EventNodeVO)
			{
				if (Selected is EventNodeVO && Selected != component)
					menu.AddItem(new GUIContent("Link to Moment"), false, LinkMoment, Selected);
				menu.AddItem(new GUIContent("Delete Moment"), false, DeleteComponent, component);
			}
			else if (component is EventLinkVO)
				menu.AddItem(new GUIContent("Delete Link"), false, DeleteComponent, component);
			menu.ShowAsContext();
		}

		void CreateNewNode(object fromNode)
		{
			EventNodeVO node = new EventNodeVO("New Moment");
			node.Position = _mousePos;
			_nodes.Add(node);
			CreateLink(fromNode as EventNodeVO, node);
			Selected = node; 
			_dirty = true;
		}

		void LinkMoment(object obj)
		{
			Selected = CreateLink(obj as EventNodeVO, Selected as EventNodeVO);
		}

		private EventLinkVO CreateLink(EventNodeVO start, EventNodeVO end)
		{
			EventLinkVO link = null;
			if (start != null && end != null)
			{
				link = new EventLinkVO(start, end);
				link.Start = start;
				link.End = end;
				_links.Add(link);
				_dirty = true;
			}
			return link;
	    }

		private void AdjustScrollRect(Rect rect)
		{
			if (rect.xMin < _scrollRect.xMin) _scrollRect.xMin = rect.xMin;
			else if (rect.xMax > _scrollRect.xMax) _scrollRect.xMax = rect.xMax;
			if (rect.yMin < _scrollRect.yMin) _scrollRect.yMin = rect.yMin;
			else if (rect.yMax > _scrollRect.yMax) _scrollRect.yMax = rect.yMax;
		}
	}
}
  