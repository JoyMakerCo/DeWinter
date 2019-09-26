#if (UNITY_EDITOR)

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

namespace UGraph
{
    public sealed class GraphEditorWindow : EditorWindow
    {
        private const float LINK_WIDTH = 5f;
        private const float NODE_WIDTH = 200f;
        private const float NODE_HEIGHT = 40f;
        private const float SCROLL_PADDING = 40f;

        private DirectedGraphConfig _config;
        private SerializedObject _object;
        private SerializedProperty _graph;
        private Vector2 _scroll;
        private Rect _scrollRect;
        private Vector2 _mousePos;
        private bool _relinking;
        private int _linking;
        private bool _mouseDown = false;

        private List<int> _selectedNodes;
        private List<int> _selectedLinks;

        private SerializedProperty _nodes;
        private SerializedProperty _links;
        private SerializedProperty _linkData;
        private SerializedProperty _positions;

        public static GraphEditorWindow Show(DirectedGraphConfig config)
        {
            string title = (config == null ? "" : (config.name + " | ")) +  "Graph Editor";
            GraphEditorWindow window = GetWindow<GraphEditorWindow>(title, true, typeof(SceneView));
            window.SetObject(config);
            EditorApplication.update -= window.UpdateGraphEditorWindow;
            if (config != null) EditorApplication.update += window.UpdateGraphEditorWindow;
            return window;
        }

        private void UpdateGraphEditorWindow()
        {
            if (_selectedLinks.Count + _selectedNodes.Count > 0)
            {
                _object?.Update();
                Repaint();
            }
        }

        private void SetObject(DirectedGraphConfig config)
        {
            if (config == null || _config == config)
            {
                _object?.Update();
                return;
            }
            _config = config;
            _object = new SerializedObject(_config);
            _object.Update();
            _graph = _object.FindProperty(_config.GraphProperty);
            if (_graph == null)
            {
                _object = null;
                _config = null;
                return;
            }

            Vector2 adjust;
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            _config.InitializeEditor(_object);
            _scrollRect = rect;
            _nodes = _graph.FindPropertyRelative("Nodes");
            _links = _graph.FindPropertyRelative("Links");
            _linkData = _graph.FindPropertyRelative("LinkData");
            _positions = _object.FindProperty("_Positions");

            if ((_nodes?.arraySize ?? 0) == 0)
            {
                CreateNewNode(Vector2.zero);
            }
            _positions.arraySize = _nodes.arraySize;
            adjust = _positions.GetArrayElementAtIndex(0).vector2Value;

            for (int i = _positions.arraySize - 1; i >= 0; i--)
            {
                rect.center = _positions.GetArrayElementAtIndex(i).vector2Value -= adjust;
                AdjustScrollRect(rect, SCROLL_PADDING);
            }

            _object.ApplyModifiedProperties();
            _selectedNodes = new List<int>();
            _selectedLinks = new List<int>();
            _linking = -1;

            _scroll = new Vector2((position.width - _scrollRect.xMin)*.5f, -position.height - _scrollRect.yMin*.5f);
            Repaint();
        }

        void OnSelectionChange()
        {
            DirectedGraphConfig config = Selection.activeObject as DirectedGraphConfig;
            if (config == null || config != _config)
            {
                OnDisable();
                Show(config);
            }
        }

        void OnDisable()
        {
            EditorApplication.update -= UpdateGraphEditorWindow;
            _config?.CleanupEditor(_object);
            _object?.ApplyModifiedProperties();
            _config = null;
            _object = null;
            _graph = null;
            titleContent.text = "Graph Editor";
            Repaint();
        }

        public void OnGUI()
        {
            if (_object != null)
            {
                int index;
                bool shift = Event.current.shift;
                Vector2 pos = Event.current.mousePosition;
                Rect viewRect = position;
                viewRect.x = viewRect.y = 0f;
                _object.Update();
                _scroll = GUI.BeginScrollView(viewRect, _scroll, _scrollRect);

                switch (Event.current.type)
                {
                    case EventType.KeyDown:
                        if ((Event.current.modifiers & (EventModifiers.Command | EventModifiers.Control)) > 0
                            && (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete))
                        {
                            DeleteSelected();
                        }
                        else if (Event.current.modifiers == EventModifiers.None && _selectedNodes.Count + _selectedLinks.Count > 0)
                        {
                            EditorWindow win = Array.Find(Resources.FindObjectsOfTypeAll<EditorWindow>(), w => w.titleContent.text == "Inspector");
                            if (win != null) win.Focus();
                        }
                        break;
                    case EventType.MouseDown:
                        _mouseDown = true;
                        _mousePos = Event.current.mousePosition;
                        if (!shift)
                        {
                            _selectedNodes.Clear();
                            _selectedLinks.Clear();
                        }
                        _linking = IntersectNode(_mousePos);
                        if (_linking >= 0)
                        {
                            if (!_selectedNodes.Remove(_linking))
                                _selectedNodes.Add(_linking);
                        }
                        else
                        {
                            _linking = IntersectLink(_mousePos);
                            if (_linking >= 0 && !_selectedLinks.Remove(_linking))
                            {
                                _selectedLinks.Add(_linking);
                            }
                        }
                        if (Event.current.button != 1)
                            _linking = -1;
                        break;

                    case EventType.MouseDrag:
                        if (_mouseDown && _selectedLinks.Count + _selectedNodes.Count > 0)
                        {
                            switch (Event.current.button)
                            {
                                // Drag nodes with lmb
                                case 0:
                                    Vector2 offset = Event.current.mousePosition - _mousePos;
                                    Rect rect = new Rect(0, 0, NODE_WIDTH, NODE_HEIGHT);
                                    _relinking = false;
                                    foreach(int i in _selectedNodes)
                                    {
                                        pos = _positions.GetArrayElementAtIndex(i).vector2Value;
                                        pos += offset;
                                        _positions.GetArrayElementAtIndex(i).vector2Value = pos;

                                        rect.position = _positions.GetArrayElementAtIndex(i).vector2Value;
                                        AdjustScrollRect(rect, SCROLL_PADDING);
                                    }
                                    break;

                                // Reposition links/Create nodes with rmb
                                case 1:
                                    _relinking &= _linking >= 0 && _linking < _links.arraySize;
                                    break;
                            }
                        } // else -- TODO: Marquis select multiple objects. Also support multiple selections.
                        _mousePos = Event.current.mousePosition;
                        break;

                    case EventType.MouseUp:
                        _mouseDown = false;
                        _mousePos = Event.current.mousePosition;
                        if (_linking >= 0)
                        {
                            index = IntersectNode(_mousePos);
                            if (index < 0) index = CreateNewNode(_mousePos);
                            if (_relinking) RedirectLink(_linking, index);
                            else Link(_linking, index);
                            if (_selectedNodes.Count == 1 && _selectedLinks.Count == 0)
                                _selectedNodes[0] = index;
                            else _selectedNodes.Add(index);
                        }
                        _relinking = false;
                        _linking = -1;
                        break;

                    case EventType.MouseMove:
                        _mouseDown = false;
                        break;
                }

                _object.FindProperty("_Nodes").arraySize = _selectedNodes.Count;
                _object.FindProperty("_Links").arraySize = _selectedLinks.Count;
                for(int i=Math.Max(_selectedLinks.Count, _selectedNodes.Count)-1; i>=0; i--)
                {
                    if (i < _selectedNodes.Count)
                    {
                        _object.FindProperty("_Nodes").GetArrayElementAtIndex(0).intValue = _selectedNodes[i];
                    }
                    if (i < _selectedLinks.Count)
                    {
                        _object.FindProperty("_Links").GetArrayElementAtIndex(0).intValue = _selectedLinks[i];
                    }
                }

                _object.ApplyModifiedProperties();
                DrawLinks();
                if (_linking >= 0)
                {
                    pos = _relinking ? GetLinkEnds(_linking)[0] : _positions.GetArrayElementAtIndex(_linking).vector2Value;
                    DrawLink(pos, _mousePos, true);
                }
                DrawNodes();
                GUI.EndScrollView();
                Repaint();
            }
        }

        // Creates a new Link. Returns the index of the new link, or -1 if the indices are invalid.
        private int Link(int start, int end)
        {
            if (start == end || start < 0 || start >= _positions.arraySize || end < 0 || end >= _positions.arraySize)
                return -1;

            SerializedProperty prop;
            int index = _links.arraySize;
            _links.InsertArrayElementAtIndex(index);
            prop = _links.GetArrayElementAtIndex(index);
            prop.vector2IntValue = new Vector2Int(start, end);

            if (_linkData != null)
            {
                _linkData.arraySize = _links.arraySize - 1;
                _linkData.InsertArrayElementAtIndex(index);
                (_config as IDirectedGraphLinkInitializer)?.InitializeLink(_linkData.GetArrayElementAtIndex(index), index, start, end);
            }
            return index;
        }

        // Sets the second index of the link to the new end node. Returns null if either argument is out of bounds.
        private SerializedProperty RedirectLink(int index, int endNode)
        {
            if (_links != null && index >= 0 && endNode >= 0 && index < _links.arraySize && endNode < _positions.arraySize)
            {
                SerializedProperty prop = _links.GetArrayElementAtIndex(index);
                Vector2Int link = prop.vector2IntValue;
                link.y = endNode;
                prop.vector2IntValue = link;
                return prop;
            }
            return null;
        }

        private static bool DeleteIndex(SerializedProperty list, int index)
        {
            if (list == null || !list.isArray || index < 0 || index >= list.arraySize)
                return false;
            list.DeleteArrayElementAtIndex(index);
            if (index < list.arraySize && list.GetArrayElementAtIndex(index) == null)
                list.DeleteArrayElementAtIndex(index);
            return true;
        }

        private void AdjustScrollRect(Rect rect, float pad=0f)
        {
            _scrollRect.xMin = Math.Min(_scrollRect.xMin, rect.xMin - pad);
            _scrollRect.xMax = Math.Max(_scrollRect.xMax, rect.xMax + pad);
            _scrollRect.yMin = Math.Min(_scrollRect.yMin, rect.yMin - pad);
            _scrollRect.yMax = Math.Max(_scrollRect.yMax, rect.yMax + pad);
        }

        private void DeleteSelected()
        {
            foreach (int link in _selectedLinks)
            {
                (_config as IDirectedGraphDeleteLinkHandler)?.DeleteLink(_linkData?.GetArrayElementAtIndex(link), link);
                DeleteIndex(_links, link);
                DeleteIndex(_linkData, link);
            }
            _selectedLinks.Clear();

            foreach (int node in _selectedNodes)
            {
                (_config as IDirectedGraphDeleteNodeHandler)?.DeleteNode(_nodes?.GetArrayElementAtIndex(node), node);
                DeleteIndex(_nodes, node);
                DeleteIndex(_positions, node);
                if (_links != null)
                {
                    Vector2Int vec;
                    for (int i=_links.arraySize-1; i>=0; i--)
                    {
                        vec = _links.GetArrayElementAtIndex(i).vector2IntValue;
                        if (vec.x == node || vec.y == node)
                        {
                            (_config as IDirectedGraphDeleteLinkHandler)?.DeleteLink(_links.GetArrayElementAtIndex(i), i);
                            DeleteIndex(_links, i);
                            DeleteIndex(_linkData, i);
                            _selectedLinks.Remove(i);
                        }
                        else
                        {
                            if (vec.x >= node) vec.x--;
                            if (vec.y >= node) vec.y--;
                            _links.GetArrayElementAtIndex(i).vector2IntValue = vec;
                        }
                    }
                }
            }
            _selectedNodes.Clear();
        }

        private int CreateNewNode(Vector2 position)
        {
            _nodes = _nodes ?? _graph?.FindPropertyRelative("Nodes");
            if (_nodes == null) return -1;
            int index = _nodes.arraySize++;
            (_config as IDirectedGraphNodeInitializer)?.InitializeNode(_nodes.GetArrayElementAtIndex(index), index);
            if (_positions == null) return index;
            _positions.arraySize = _nodes.arraySize;
            _positions.GetArrayElementAtIndex(index).vector2Value = position;
            return index;
        }

        private void DrawNodes()
        {
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            _positions.arraySize = _nodes.arraySize;
            if (_config is IDirectedGraphNodeRenderer)
            {
                for (int i = _positions.arraySize - 1; i >= 0; i--)
                {
                    rect.center = _positions.GetArrayElementAtIndex(i).vector2Value;
                    (_config as IDirectedGraphNodeRenderer).Render(_nodes.GetArrayElementAtIndex(i), i, rect, _selectedNodes.Contains(i));
                }
            }
            else for (int i = _positions.arraySize - 1; i >= 0; i--)
            {
                rect.center = _positions.GetArrayElementAtIndex(i).vector2Value;
                DrawNode(rect, _selectedNodes.Contains(i));
            }
        }

        private void DrawLinks()
        {
            Vector2[] link;
            for (int i = _links.arraySize - 1; i >= 0; i--)
            {
                if (!_relinking || i != _linking)
                {
                    link = GetLinkEnds(i);
                    DrawLink(link[0], link[1], _selectedLinks.Contains(i));
                }
            }
        }

        private bool DrawNode(Rect rect, bool selected)
        {
            return GUI.Toggle(rect, selected, new GUIContent(), GUI.skin.button);
        }

        private void DrawLink(Vector2 from, Vector2 to, bool selected)
        {
            Vector3 dir = (to - from).normalized * LINK_WIDTH;
            Vector3 norm = new Vector2(dir.y, -dir.x);
            Vector3 mid = (from + to) * .5f;

            // ...AND I'LL FORM THE HEAD
            Vector3[] head = {
                mid - dir - norm,
                mid + dir,
                mid - dir + norm
            };
            Handles.color = selected ? Color.white : Color.black;
            Handles.DrawLine(from, to);
            Handles.DrawAAConvexPolygon(head);
        }

        /// <returns>The index of the node intersected by the mouse, or -1 if none were intersected</returns>
        private int IntersectNode(Vector2 mousepos)
        {
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            for (int index = _positions.arraySize - 1; index >= 0; index--)
            {
                rect.center = _positions.GetArrayElementAtIndex(index).vector2Value;
                if (rect.Contains(mousepos)) return index;
            }
            return -1;
        }

        private int IntersectLink(Vector2 mousePos)
        {
            Vector2[] link;
            Vector2 p;
            float P;
            Vector2 l;
            float L;
            float dot;
            for (int index = _links.arraySize - 1; index >= 0; index--)
            {
                link = GetLinkEnds(index);
                p = mousePos - link[0];
                P = p.sqrMagnitude;
                l = (link[1] - link[0]);
                L = l.sqrMagnitude;
                dot = Vector2.Dot(p, l);
                if ((dot >= 0) && (dot * dot <= L * L)
                    && (Math.Abs(P - (dot * dot / L)) <= LINK_WIDTH * LINK_WIDTH))
                {
                    return index;
                }
            }
            return -1;
        }

        // This will throw an error if the index is invalid
        private Vector2[] GetLinkEnds(int index)
        {
            Vector2Int ln = _links.GetArrayElementAtIndex(index).vector2IntValue;
            Vector2 from = _positions.GetArrayElementAtIndex(ln.x).vector2Value;
            Vector2 to = _positions.GetArrayElementAtIndex(ln.y).vector2Value;
            return new Vector2[] { from, to };
        }

        [MenuItem("Window/Utilities/Graph Editor")]
        private static void GraphEditorWindowMenuItem()
        {
            GetWindow<GraphEditorWindow>("Graph Editor", true);
        }
    }
}

#endif
