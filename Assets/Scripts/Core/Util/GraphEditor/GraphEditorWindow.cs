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
        private const float SCROLL_PADDING = 40f;

        private IDirectedGraphObjectConfig _graphObject;
        private Vector2 _scroll;
        private Rect _scrollRect;
        private Vector2 _mousePos;
        private Vector2 _tempPos;
        private bool _relinking;

        private int _selection;
        private bool _isNode;

        private SerializedProperty _nodes;
        private SerializedProperty _links;
        private SerializedProperty _linkData;
        private SerializedProperty _positions;

        public static GraphEditorWindow Show(IDirectedGraphObjectConfig graphObject)
        {
            if (graphObject is ScriptableObject)
            {
                string str = (graphObject as ScriptableObject).name;
                GraphEditorWindow window = GetWindow<GraphEditorWindow>(str + " | Graph Editor");
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
                _graphObject.GraphObject.Update();
                Repaint();
            }
        }

        private void SetObject(IDirectedGraphObjectConfig graphObject)
        {
            Vector2 adjust;
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            _graphObject = graphObject;
            _scrollRect = new Rect();
            _graphObject.GraphObject.Update();

            _nodes = _graphObject.GraphProperty.FindPropertyRelative("Nodes");
            _links = _graphObject.GraphProperty.FindPropertyRelative("Links");
            _linkData = _graphObject.GraphProperty.FindPropertyRelative("LinkData");
            _positions = _graphObject.GraphProperty.FindPropertyRelative("Positions");

            if (_nodes.arraySize == 0) CreateNewNode(Vector2.zero);
            _positions.arraySize = _nodes.arraySize;
            adjust = _positions.GetArrayElementAtIndex(0).vector2Value;
            for (int i = _positions.arraySize - 1; i >= 0; i--)
            {
                rect.center = _positions.GetArrayElementAtIndex(i).vector2Value -= adjust;
                AdjustScrollRect(rect, SCROLL_PADDING);
            }
            _graphObject.GraphObject.ApplyModifiedProperties();
            _isNode = true;
            _selection = -1;
            _scroll = new Vector2((position.width - _scrollRect.xMin)*.5f, -position.height - _scrollRect.yMin*.5f);
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
                _graphObject.GraphObject.Update();
                _scroll = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), _scroll, _scrollRect);
                switch (Event.current.type)
                {
                    case EventType.KeyDown:
                        if ((Event.current.modifiers & (EventModifiers.Command | EventModifiers.Control)) > 0
                            && (Event.current.keyCode == KeyCode.Backspace || Event.current.keyCode == KeyCode.Delete))
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
                        _selection = IntersectNode(_mousePos);
                        _isNode = _selection >= 0;
                        if (!_isNode) _selection = IntersectLink(_mousePos);
                        break;

                    case EventType.MouseDrag:
                        if (_selection >= 0)
                        {
                            switch (Event.current.button)
                            {
                                // Drag nodes with lmb
                                case 0:
                                    if (_isNode)
                                    {
                                        _tempPos = _positions.GetArrayElementAtIndex(_selection).vector2Value;
                                        _tempPos += Event.current.mousePosition - _mousePos;
                                        _positions.GetArrayElementAtIndex(_selection).vector2Value = _tempPos;
                                    }
                                    else if (!_relinking)
                                    {
                                        _relinking = true;
                                        _tempPos = GetLinkEnds(_selection)[0];
                                    }
                                    break;

                                // Reposition links/Create nodes with rmb
                                case 1:
                                    if (!_relinking)
                                    {
                                        _relinking = true;
                                        _tempPos = _isNode
                                            ? _positions.GetArrayElementAtIndex(_selection).vector2Value
                                            : GetLinkEnds(_selection)[0];
                                    }
                                    break;
                            }
                        } // else -- TODO: Marquis select multiple objects. Also support multiple selections.
                        _mousePos = Event.current.mousePosition;
                        break;

                    case EventType.MouseUp:
                        _mousePos = Event.current.mousePosition;
                        if (_relinking)
                        {
                            index = IntersectNode(_mousePos);
                            // Create a new node if right-dragging
                            if (index < 0 && Event.current.button == 1)
                                index = CreateNewNode(_mousePos);
                            if (_isNode) // Linking to a new node
                            {
                                Link(_selection, index);
                                if (index >= 0) _selection = index;
                            }
                            else // Redirecting an existing link
                            {
                                // Redirecting a link to its own source deletes the link
                                if (GetLink(_selection).x == index) DeleteSelected();

                                // Redirect the Link 
                                else RedirectLink(_selection, index);
                            }
                            _relinking = false;
                        }
                        break;
                }
                if (_graphObject != null)
                    _graphObject.Select(_selection, _isNode);
                _graphObject.GraphObject.ApplyModifiedProperties();
                DrawLinks();
                if (_relinking) DrawLink(_tempPos, _mousePos, !_isNode);
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
            int index = _links.arraySize++;
            _links.InsertArrayElementAtIndex(index);
            prop = _links.GetArrayElementAtIndex(index);
            prop.vector2IntValue = new Vector2Int(start, end);

            if (_linkData != null)
            {
                int size = _linkData.arraySize;
                _linkData.arraySize = _links.arraySize;
                for (int i = _linkData.arraySize-1; i >= size; i--)
                {
                    _linkData.InsertArrayElementAtIndex(i);
                    _graphObject.InitLinkData(_linkData.GetArrayElementAtIndex(index));
                }
            }
            return index;
        }

        // Sets the second index of the link to the new end node. Returns null if either argument is out of bounds.
        private SerializedProperty RedirectLink(int index, int endNode)
        {
            if (index >= 0 && endNode >= 0 && index < _links.arraySize && endNode < _positions.arraySize)
            {
                SerializedProperty prop = _links.GetArrayElementAtIndex(_selection);
                Vector2Int link = prop.vector2IntValue;
                link.y = endNode;
                prop.vector2IntValue = link;
                return prop;
            }
            return null;
        }

        private static bool DeleteIndex(SerializedProperty graphProperty, string listID, int index)
        {
            if (graphProperty == null) return false;
            SerializedProperty list = graphProperty.FindPropertyRelative(listID);
            if (list == null || list.arraySize <= index) return false;
            list.DeleteArrayElementAtIndex(index);
            if (index < list.arraySize && list.GetArrayElementAtIndex(index) == null)
                list.DeleteArrayElementAtIndex(index);
            return true;
        }

        private void AdjustScrollRect(Rect rect, float pad=0f)
        {
            if (rect.xMin - SCROLL_PADDING < _scrollRect.xMin)
            {
                _scrollRect.xMin = rect.xMin - SCROLL_PADDING;
            }
            else if (rect.xMax + SCROLL_PADDING > _scrollRect.xMax)
            {
                _scrollRect.xMax = rect.xMax + SCROLL_PADDING;
            }
            if (rect.yMin - SCROLL_PADDING < _scrollRect.yMin)
            {
                _scrollRect.yMin = rect.yMin - SCROLL_PADDING;
            }
            else if (rect.yMax + SCROLL_PADDING > _scrollRect.yMax)
            {
                _scrollRect.yMax = rect.yMax + SCROLL_PADDING;
            }
        }

        private bool DeleteSelected()
        {
            return DeleteSelected(_graphObject, _selection, _isNode);
        }

        public static bool DeleteSelected(IDirectedGraphObjectConfig graphObject, int index, bool isNode)
        {
            if (index < 0 || graphObject == null || graphObject.GraphProperty == null) return false;
            SerializedProperty links = graphObject.GraphProperty.FindPropertyRelative("Links");
            SerializedProperty graphProperty = graphObject.GraphProperty;
            if (isNode)
            {
                SerializedProperty nodes = graphProperty.FindPropertyRelative("Nodes");
                if (nodes == null || nodes.arraySize <= 1) return false;
                DeleteIndex(graphProperty, "Positions", index);
                if (!DeleteIndex(graphProperty, "Nodes", index)) return false;

                Vector2Int ln;
                for (int i = links.arraySize - 1; i >= 0; i--)
                {
                    ln = links.GetArrayElementAtIndex(i).vector2IntValue;
                    if (ln.x == index || ln.y == index)
                    {
                        DeleteIndex(graphProperty, "Links", i);
                        DeleteIndex(graphProperty, "LinkData", i);
                    }
                    else
                    {
                        if (ln.x > index) ln.x--;
                        if (ln.y > index) ln.y--;
                        links.GetArrayElementAtIndex(i).vector2IntValue = ln;
                    }
                }
            }
            else
            {
                DeleteIndex(graphProperty, "LinkData", index);
                if (!DeleteIndex(graphProperty, "Links", index)) return false;
            }
            graphObject.Select(-1, false);
            GraphEditorWindow window = GetWindow<GraphEditorWindow>(((ScriptableObject)graphObject).name + " | Graph Editor", false);
            window._isNode = false;
            window._selection = -1;
            graphObject.GraphObject.ApplyModifiedProperties();
            return true;
        }

        private int CreateNewNode(Vector2 position)
        {
            int index = _nodes.arraySize++;
            _positions.arraySize = _nodes.arraySize;
            _nodes.InsertArrayElementAtIndex(index);
            _graphObject.InitNodeData(_nodes.GetArrayElementAtIndex(index));

            _positions.GetArrayElementAtIndex(index).vector2Value = position;
            return index;
        }

        private void DrawNodes()
        {
            for (int i = _positions.arraySize - 1; i >= 0; i--)
            {
                DrawNode(i, _isNode && _selection == i);
            }
        }

        private void DrawLinks()
        {
            Vector2[] link;
            for (int i = _links.arraySize - 1; i >= 0; i--)
            {
                if (!_relinking || _isNode || i != _selection)
                {
                    link = GetLinkEnds(i);
                    DrawLink(link[0], link[1], !_isNode && _selection == i);
                }
            }
        }

        private bool DrawNode(int index, bool selected)
        {
            GUIContent content = _graphObject.GetGUIContent(index);
            Rect rect = new Rect(0f, 0f, NODE_WIDTH, NODE_HEIGHT);
            rect.center = _positions.GetArrayElementAtIndex(index).vector2Value;
            AdjustScrollRect(rect, SCROLL_PADDING);
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

        // Returns the set of node indices that represents the link.
        // Will throw an exception if the argument index is out of bounds.
        private Vector2Int GetLink(int index)
        {
            return _links.GetArrayElementAtIndex(index).vector2IntValue;
        }

        // This will throw an error if the index is invalid
        private Vector2[] GetLinkEnds(int index)
        {
            Vector2Int ln = GetLink(index);
            Vector2 from = _positions.GetArrayElementAtIndex(ln.x).vector2Value;
            Vector2 to = _positions.GetArrayElementAtIndex(ln.y).vector2Value;
            return new Vector2[] { from, to };
        }
    }
}

#endif
