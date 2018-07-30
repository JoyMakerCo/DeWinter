#if (UNITY_EDITOR)

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Util;

namespace Util
{
    public sealed class GraphEditorWindow : EditorWindow
    {
        private const float LINK_WIDTH = 5f;
        private const float NODE_WIDTH = 200f;
        private const float NODE_HEIGHT = 40f;

        private IDirectedGraphObject _graphObject;
        private Vector2 _scroll;
        private Rect _scrollRect;
        private Vector2 _mousePos;

        private int _selection;
        private bool _isNode;
        private bool _dragLink;

        private SerializedProperty _nodes;
        private SerializedProperty _links;
        private SerializedProperty _linkData;
        private SerializedProperty _positions;

        public static GraphEditorWindow Show(IDirectedGraphObject graphObject)
        {
            if (graphObject is ScriptableObject)
            {
                string str = (graphObject as ScriptableObject).name;
                GraphEditorWindow window = EditorWindow.GetWindow<GraphEditorWindow>(str + " | Graph Editor");
                window.SetObject(graphObject);
                EditorApplication.update -= window.UpdateGraphEditorWindow;
                EditorApplication.update += window.UpdateGraphEditorWindow;
                return window;
            }
            return null;
        }

        private void UpdateGraphEditorWindow()
        {
            if (_graphObject != null && _selection >= 0)
            {
                _graphObject.UpdateGraph();
                Repaint();
            }
        }

        private void SetObject(IDirectedGraphObject graphObject)
        {
            _graphObject = graphObject;
            _scrollRect = new Rect();
            _nodes = graphObject.GetNodes();
            _links = graphObject.GetLinks();
            _linkData = graphObject.GetLinkData();
            _positions = graphObject.GetPositions();
            _dragLink = false;
            _selection = -1;
            if (_positions.arraySize == 0)
                CreateNewNode();
            Repaint();
        }

        void OnSelectionChange()
        {
            OnDestroy();
        }

        void OnDestroy()
        {
            EditorApplication.update -= UpdateGraphEditorWindow;
            _graphObject = null;
            titleContent.text = "Graph Editor";
            Repaint();
        }

        public void OnGUI()
        {
            if (_graphObject != null)
            {
                int index;
                Vector2Int link;
                Vector2 pos;
                _graphObject.UpdateGraph();
                _scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scroll, _scrollRect);
                switch (Event.current.type)
                {
                    case EventType.KeyDown:
                        if ((Event.current.modifiers & EventModifiers.Command) > 0 && Event.current.keyCode == KeyCode.Backspace)
                        {
                            DeleteSelected();
                        }
                        else if (Event.current.modifiers == EventModifiers.None && _selection >= 0)
                        {
                            EditorWindow win = Array.Find(Resources.FindObjectsOfTypeAll<EditorWindow>(), w => w.titleContent.text == "Inspector");
                            if (win != null) win.Focus();
                        }
                        break;
                    case EventType.MouseDown:
                        _mousePos = Event.current.mousePosition;
                        if (!IntersectNode()) IntersectLink();
                        break;

                    case EventType.MouseDrag:
                        if (_selection >= 0)
                        {
                            // Drag nodes with lmb
                            if (Event.current.button == 0)
                            {
                                if (_isNode)
                                {
                                    pos = _positions.GetArrayElementAtIndex(_selection).vector2Value;
                                    pos += Event.current.mousePosition - _mousePos;
                                    _positions.GetArrayElementAtIndex(_selection).vector2Value = pos;
                                }
                            }
                            // Relink/Create new links with rmb
                            else if (!_dragLink)
                            {
                                _dragLink = Event.current.button == 1 && _selection >= 0;
                            }
                        }
                        _mousePos = Event.current.mousePosition;
                        break;

                    case EventType.MouseUp:
                        _mousePos = Event.current.mousePosition;
                        if (_selection >= 0 && Event.current.button == 1)
                        {
                            bool wasNode = _isNode;
                            index = _selection;
                            if (!IntersectNode()) CreateNewNode();
                            if (index != _selection)
                            {
                                if (wasNode)
                                {
                                    Link(index, _selection);
                                }
                                else
                                {
                                    link = _links.GetArrayElementAtIndex(index).vector2IntValue;
                                    link.y = _selection;
                                    _links.GetArrayElementAtIndex(index).vector2IntValue = link;
                                }
                            }
                            _isNode = wasNode;
                            _selection = index;
                            _dragLink = false;
                        }
                        break;
                }
                _graphObject.Select(_selection, _isNode);
                _graphObject.ApplyModifiedProperties();
                if (_dragLink)
                {
                    pos = _isNode
                            ? _positions.GetArrayElementAtIndex(_selection).vector2Value
                            : GetLinkEnds(_selection)[0];
                    DrawLink(pos, _mousePos, true);
                }
                DrawLinks();
                DrawNodes();
                GUI.EndScrollView();
                Repaint();
            }
        }

        private void Link(int start, int end)
        {
            if (start != end && start >= 0 && start < _positions.arraySize && end >= 0 && end < _positions.arraySize)
            {
                _selection = _links.arraySize++;
                _links.InsertArrayElementAtIndex(_selection);
                _links.GetArrayElementAtIndex(_selection).vector2IntValue = new Vector2Int(start, end);

                if (_linkData != null)
                {
                    int index;
                    while (_linkData.arraySize < _links.arraySize)
                    {
                        index = _linkData.arraySize++;
                        _linkData.InsertArrayElementAtIndex(index);
                        _graphObject.InitLinkData(_linkData.GetArrayElementAtIndex(index));
                    }
                }
                _isNode = false;
            }
        }

        private bool DeleteIndex(SerializedProperty list, int index)
        {
            if (list == null || index >= list.arraySize) return false;
            list.DeleteArrayElementAtIndex(index);
            if (index < list.arraySize && list.GetArrayElementAtIndex(index) == null)
                list.DeleteArrayElementAtIndex(index);
            return true;
        }

        private void AdjustScrollRect(Vector2 point)
        {
            if (point.x < _scrollRect.xMin)
            {
                _scrollRect.xMin = point.x;
            }
            else if (point.x > _scrollRect.xMax)
            {
                _scrollRect.xMax = point.x;
            }
            if (point.y < _scrollRect.yMin)
            {
                _scrollRect.yMin = point.y;
            }
            else if (point.y > _scrollRect.yMax)
            {
                _scrollRect.yMax = point.y;
            }
        }

        private void DeleteSelected()
        {
            if (_selection < 0) return;
            if (_isNode)
            {
                DeleteIndex(_nodes, _selection);
                DeleteIndex(_positions, _selection);

                Vector2Int ln;
                for (int i = _links.arraySize - 1; i >= 0; i--)
                {
                    ln = _links.GetArrayElementAtIndex(i).vector2IntValue;
                    if (ln.x == _selection || ln.y == _selection)
                    {
                        DeleteIndex(_links, i);
                        DeleteIndex(_linkData, i);
                    }
                    else
                    {
                        if (ln.x >= _selection) ln.x -= 1;
                        if (ln.y >= _selection) ln.y -= 1;
                        _links.GetArrayElementAtIndex(i).vector2IntValue = ln;
                    }
                }
            }
            else
            {
                DeleteIndex(_links, _selection);
                DeleteIndex(_linkData, _selection);
            }
            _selection = -1;
            _isNode = false;
        }

        private void CreateNewNode()
        {
            int index;
            _selection = _nodes == null ? (_positions.arraySize + 1) : (1 + Math.Max(_nodes.arraySize, _positions.arraySize));
            while (_positions.arraySize <= _selection)
            {
                _positions.arraySize++;
                _positions.InsertArrayElementAtIndex(_selection);
                _positions.GetArrayElementAtIndex(_selection).vector2Value = _mousePos;
            }

            if (_nodes != null)
            {
                while (_nodes.arraySize < _selection)
                {
                    index = _nodes.arraySize++;
                    _nodes.InsertArrayElementAtIndex(index);
                    _graphObject.InitNodeData(_nodes.GetArrayElementAtIndex(index));
                }
            }
            _isNode = true;
        }

        private void DrawNodes()
        {
            for (int i = _positions.arraySize - 1; i >= 0; i--)
            {
                if (DrawNode(i, _isNode && _selection == i))
                {
                    _selection = i;
                    _isNode = true;
                }
            }
        }

        private void DrawLinks()
        {
            Vector2[] link;
            bool selected;
            for (int i = _links.arraySize - 1; i >= 0; i--)
            {
                selected = !_isNode && _selection == i;
                if (!_dragLink || !selected)
                {
                    link = GetLinkEnds(i);
                    DrawLink(link[0], link[1], selected);
                }
            }
        }

        private bool DrawNode(int index, bool selected)
        {
            GUIContent content = _graphObject.GetGUIContent(index);
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            rect.center = _positions.GetArrayElementAtIndex(index).vector2Value;
            AdjustScrollRect(rect.center);
            return GUI.Toggle(rect, selected, content, GUI.skin.button);
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

        private bool IntersectNode()
        {
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            for (_selection = _positions.arraySize - 1; _selection >= 0; _selection--)
            {
                rect.center = _positions.GetArrayElementAtIndex(_selection).vector2Value;
                _isNode = rect.Contains(_mousePos);
                if (_isNode) return true;
            }
            return false;
        }

        private bool IntersectLink()
        {
            Vector2[] link;
            Vector2 p;
            float P;
            Vector2 l;
            float L;
            float dot;
            for (_selection = _links.arraySize - 1; _selection >= 0; _selection--)
            {
                link = GetLinkEnds(_selection);
                p = _mousePos - link[0];
                P = p.sqrMagnitude;
                l = (link[1] - link[0]);
                L = l.sqrMagnitude;
                dot = Vector2.Dot(p, l);
                if ((dot >= 0) && (dot * dot <= L * L)
                    && (Math.Abs(P - (dot * dot / L)) <= LINK_WIDTH * LINK_WIDTH))
                {
                    _isNode = false;
                    return true;
                }
            }
            return false;
        }

        private Vector2[] GetLinkEnds(int index)
        {
            Vector2Int ln = _links.GetArrayElementAtIndex(index).vector2IntValue;
            Vector2 from = _positions.GetArrayElementAtIndex(ln.x).vector2Value;
            Vector2 to = _positions.GetArrayElementAtIndex(ln.y).vector2Value;
            return new Vector2[] { from, to };
        }
    }
}

#endif
