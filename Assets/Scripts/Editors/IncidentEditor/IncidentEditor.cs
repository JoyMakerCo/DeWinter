#if (UNITY_EDITOR)
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;  
using UnityEditor;
using Util;

namespace Ambition
{
	public class IncidentEditor : EditorWindow
	{
		private const int NUM_VISIBLE_LINES = 4;
		private const float CONNECT_BTN_WIDTH = 10f;

		private static IncidentEditor _editor;

		private SerializedObject _collection;
		private SerializedProperty _config;
		private Vector2 _scroll;
		private Rect _scrollRect;
		private Vector2 _mousePos;		
		private bool _dragging=false;
		private bool _dirty=false;
		private IGraphComponent _selected;

		private List<IncidentNodeVO> _nodes = new List<IncidentNodeVO>();
		private List<IncidentLinkVO> _links = new List<IncidentLinkVO>();

	    public static void Show(SerializedObject obj)
	    {
			_editor = EditorWindow.GetWindow<IncidentEditor>();
	    	if (obj != null && obj.targetObject is IncidentCollection)
	    	{
				_editor.SetCollection(obj);
			}
			else _editor.OnDestroy();
		}
		
		public static void Show(IncidentCollection collection)
	    {
			_editor = EditorWindow.GetWindow<IncidentEditor>();
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
						IGraphComponent comp = _nodes.Find(m=>m.Intersect(m.Position, _mousePos));
						if (comp == null) comp = _links.Find(l=>l.Intersect(l.Start.Position, l.End.Position, _mousePos));
						else _dragging = Event.current.button == 0;
						if (Event.current.button == 1) ShowMenu(comp);
						if (Selected != comp) _dirty = true;
						Selected = comp;
						break;

					case EventType.MouseDrag:
						if (_dragging)
						{
							((IncidentNodeVO)Selected).Position += Event.current.mousePosition - _mousePos;
							_dirty = true;
							_mousePos = Event.current.mousePosition;
						}
						break;

					case EventType.MouseUp:
						_dirty = _dragging;
						_dragging = false;
						_mousePos = Event.current.mousePosition;
						break;
				}
				if (_dragging) AdjustScrollRect(((IncidentNodeVO)Selected).Rect);
				_links.ForEach(l => l.OnGUI(l.Start.Position, l.End.Position, l == Selected));
				DrawStartOutline();
				_nodes.ForEach(m => m.OnGUI(m.Position, m == Selected));
				if (_dirty)
				{
					Reserialize();
					Repaint();
					_dirty = false;
				}
				_collection.ApplyModifiedProperties();
				GUI.EndScrollView();
			}
	    }

	    private IGraphComponent Selected
	    {
	    	get { return _selected; }
	    	set {
				bool isNode = value is IGraphNodeComponent;
				int index = (value == null ? -1 : isNode ? _nodes.IndexOf(value as IncidentNodeVO) : _links.IndexOf(value as IncidentLinkVO));
	    		_collection.FindProperty(IncidentCollection.SELECTED_COMPONENT).intValue = index;
				_collection.FindProperty(IncidentCollection.IS_MOMENT).boolValue = true;
				_dirty = _dirty || (_selected != value);
	    		_selected = value;
				_collection.ApplyModifiedProperties();
	    	}
	    }

		private void SetAsStartNode(object nodeObj)
		{
			if (nodeObj is IGraphNodeComponent && _nodes.Count > 0)
			{
				int index = _nodes.IndexOf(nodeObj as IncidentNodeVO);
				if (index > 0)
				{
					_nodes.RemoveAt(index);
					_nodes.Insert(0, (IncidentNodeVO)Selected);
					_dirty = true;
				}
			}
		}

		private void DeleteSelected()
		{
			if (!_links.Remove(Selected as IncidentLinkVO))
			{
				int index = _nodes.IndexOf(Selected as IncidentNodeVO);
				if (_nodes.Remove(Selected as IncidentNodeVO))
				{
					SerializedProperty prop = _config.FindPropertyRelative("Moments");
					if (prop != null)
					{
						prop.DeleteArrayElementAtIndex(index);
						_collection.ApplyModifiedProperties();
					}
					_links.RemoveAll(l=>l.Start == Selected || l.End == Selected);
				}
			}
			Selected = null;
		}

		private void SetCollection(SerializedObject obj)
		{
			IncidentCollection collection;
			IncidentVO config;
			int index;

			_collection = obj;
			collection = (IncidentCollection)(_collection.targetObject);
			index = collection._selectedIncident;
			config = collection.Incidents.Length > index ? collection.Incidents[index] : null;
			if (config != null)
			{
				int count = config.Moments == null ? 0 : config.Moments.Length;
				IncidentNodeVO node;
				IncidentLinkVO link;

				_config = obj.FindProperty("Incidents").GetArrayElementAtIndex(index);

				titleContent.text = "Incident Editor - " + config.Name;
				_nodes = new List<IncidentNodeVO>();
				_scrollRect = new Rect();
				for (int i=0; i<count; i++)
				{
					node = new IncidentNodeVO(config.Moments[i].Text);
					node.Position = (i < config.Positions.Length) ? config.Positions[i] : Vector2.zero;
					_nodes.Add(node);
					AdjustScrollRect(node.Rect);
				}
				_links = new List<IncidentLinkVO>();
				count = config.Transitions.Length;
				for (int i=0; i<count; i++)
				{
					link = new IncidentLinkVO(_nodes[config.Transitions[i].Index], _nodes[config.Transitions[i].Target]);
					link.Text = config.Transitions[i].Text;
					_links.Add(link);
				}
			}
			Repaint();
	    }

	    private void Reserialize()
	    {
			SerializedProperty moments = _config.FindPropertyRelative("Moments");
			SerializedProperty positions = _config.FindPropertyRelative("Positions");
			SerializedProperty links = _config.FindPropertyRelative("Transitions");
			int count = _nodes.Count;
			IncidentLinkVO link;
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
				if (Selected is IGraphNodeComponent)
					menu.AddItem(new GUIContent("Create And Transition To New Moment"), false, CreateNewNode, Selected);
			}
			else
			{
				component.OnMenu(menu);
				if (_nodes.IndexOf(component as IncidentNodeVO) > 0)
					menu.AddItem(new GUIContent("Set as Starting Moment"), false, SetAsStartNode, component);
				if (component is IncidentNodeVO && Selected != component)
					menu.AddItem(new GUIContent("Transition to Moment"), false, LinkMoment, Selected);
			}
			menu.ShowAsContext();
		}

		void CreateNewNode(object fromNode)
		{
			IncidentNodeVO node = new IncidentNodeVO("New Moment");
			node.Position = _mousePos;
			node.DeleteNode = DeleteSelected;
			_nodes.Add(node);
			CreateLink(fromNode as IncidentNodeVO, node);
			Selected = node; 
			_dirty = true;
		}

		void LinkMoment(object obj)
		{
			Selected = CreateLink(obj as IncidentNodeVO, Selected as IncidentNodeVO);
		}

		private IncidentLinkVO CreateLink(IncidentNodeVO start, IncidentNodeVO end)
		{
			IncidentLinkVO link = null;
			if (start != null && end != null)
			{
				link = new IncidentLinkVO(start, end);
				link.Start = start;
				link.End = end;
				_links.Add(link);
				_dirty = true;
				link.DeleteLink = DeleteSelected;
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

		private void DrawStartOutline()
		{
			if (_nodes.Count > 0)
			{
				Rect rect = _nodes[0].Rect;
				rect.xMin-=2;
				rect.yMin-=2;
				rect.xMax+=2;
				rect.yMax+=2;
				GUI.color = Color.green;
				GUI.Box(rect, "");
			}
		}
	}
}
#endif
