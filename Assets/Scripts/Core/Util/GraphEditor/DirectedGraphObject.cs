using System;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace Util
{
    public abstract class DirectedGraphObject : ScriptableObject
    {
#if (UNITY_EDITOR)        
        [UnityEditor.Callbacks.OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            GraphEditorWindow.Show(Selection.activeObject as DirectedGraphObject);
            return Selection.activeObject is DirectedGraphObject;
        }

        [HideInInspector]
        public Rect[] Positions = new Rect[0];
        public static float DefaultNodeWidth = 200f;
        public static float DefaultNodeHeight = 50f;
		protected const float LINK_INTERSECT_WIDTH = 5f;

        public bool Intersect(Vector2 point, out int componentIndex, out bool isNode)
        {
            for (componentIndex = Positions.Length-1; componentIndex>=0; componentIndex--)
            {
                if (Positions[componentIndex].Contains(point))
                    return (isNode = true);
            }
            return (isNode = false);
        }

        protected abstract void DrawNode(int index, bool selected);
        protected abstract void DrawLink(int index, bool selected);

		protected virtual void DrawLink(int fromNode, int toNode, bool selected)
        {
            Vector2 from = Positions[fromNode].center;
            Vector2 to = Positions[toNode].center;
            Vector3 dir = (to-from).normalized*LINK_INTERSECT_WIDTH;
            Vector3 norm = new Vector2(dir.y, -dir.x)*LINK_INTERSECT_WIDTH;
            Vector3 mid = (to+from)*.5f;

            // ...AND I'LL FORM THE HEAD
            Vector3[] head = new Vector3[]{
                mid - dir - norm,
                mid + dir,
                mid - dir + norm
            };
            Handles.color = selected?Color.white:Color.black;
            Handles.DrawLine(from, to);
            Handles.DrawAAConvexPolygon(head);
		}
        
        protected virtual void OnMenu(GenericMenu menu, Vector2 mousePosition) {}
        protected virtual void OnNodeMenu(GenericMenu menu, int nodeIndex, bool selected, Vector2 mousePosition) {}
        protected virtual void OnLinkMenu(GenericMenu menu, int linkIndex, bool selected, Vector2 mousePosition) {}

        protected bool IntersectLink(Vector2 point, Vector2Int link)
        {
            Vector2 from = Positions[link.x].center;
            Vector2 to = Positions[link.y].center;
			Vector2 p = point - from;
			float P = p.sqrMagnitude;
            Vector2 l = (from - to);
			float L = l.sqrMagnitude;
			float dot = Vector2.Dot(p, l);
			return
				(dot >= 0) && (dot*dot/L <= L)
				&& (Math.Abs(P - (dot*dot/L)) <= LINK_INTERSECT_WIDTH*LINK_INTERSECT_WIDTH);
        }
#endif
    }

    public class DirectedGraphObject<N> : DirectedGraphObject
    {
        public DirectedGraph<N> Graph;

        public DirectedGraphObject()
        {
            Graph = new DirectedGraph<N>();
        }

#if (UNITY_EDITOR)
        override protected void DrawLink(int index, bool selected)
        {
            Vector2Int link = Graph.Links[index];
            base.DrawLink(link.x, link.y, selected);
        }

        new public bool Intersect(Vector2 point, out int componentIndex, out bool isNode)
        {
            isNode = false;
            for (componentIndex = Positions.Length-1; componentIndex>=0; componentIndex--)
            {
                if (Positions[componentIndex].Contains(point))
                    return (isNode = true);
            }
            for (componentIndex = Graph.Links.Length-1; componentIndex>=0; componentIndex--)
            {
                if (IntersectLink(point, Graph.Links[componentIndex]))
                    return true;
            }
            return false;
        }


        override protected void DrawNode(int index, bool selected)
        {
            DrawNode(Graph.Nodes[index], Positions[index], selected);
        }
		protected virtual void DrawNode(N node, Rect rect, bool selected)
		{
			GUIContent content = new GUIContent(node.ToString());
			GUIStyle style = GUI.skin.box;
            Color c = GUI.color;
			style.alignment = TextAnchor.MiddleCenter;
			GUI.color = selected?Color.yellow:Color.white;
            DrawNode(rect, content, style);
            GUI.color = c;
    	}

        protected void DrawNode(Rect position, string text)
        {
            GUI.Box(position, text);
        }
        protected void DrawNode(Rect position, Texture image)
        {
            GUI.Box(position, image);
        }
        protected void DrawNode(Rect position, GUIContent content)
        {
            GUI.Box(position, content);
        }
        protected void DrawNode(Rect position, string text, GUIStyle style)
        {
            GUI.Box(position, text, style);
        }
        protected void DrawNode(Rect position, Texture image, GUIStyle style)
        {
            GUI.Box(position, image, style);
        }
        protected void DrawNode(Rect position, GUIContent content, GUIStyle style)
        {
            GUI.Box(position, content, style);
        }

        override protected void OnMenu(GenericMenu menu, Vector2 mousePosition) {}

        override protected void OnNodeMenu(GenericMenu menu, int nodeIndex, bool selected, Vector2 mousePosition)
        {
            if (Graph != null && Graph.Nodes != null && nodeIndex < Graph.Nodes.Length)
                OnNodeMenu(menu, Graph.Nodes[nodeIndex], selected, mousePosition);
        }

        protected virtual void OnNodeMenu(GenericMenu menu, N node, bool selected, Vector2 mousePosition) {}

        override protected void OnLinkMenu(GenericMenu menu, int linkIndex, bool selected, Vector2 mousePosition)
        {
            if (Graph != null && Graph.Links != null && linkIndex < Graph.Links.Length)
            {
                Vector2Int link = Graph.Links[linkIndex];
                menu.AddItem(new GUIContent("Delete Link"), false, DeleteLink, linkIndex);
                if (Graph.Nodes != null && link.x < Graph.Nodes.Length && link.y < Graph.Nodes.Length)
                    OnLinkMenu(menu, Graph.Nodes[link.x], Graph.Nodes[link.y], selected, mousePosition);
            }
        }

        protected virtual void OnLinkMenu(GenericMenu menu, N from, N to, bool selected, Vector2 mousePosition) {}

        protected void DeleteLink(object obj)
        {
            int index = (int)obj;
            Graph.Links = Graph.Links.Take(index).Concat(Graph.Links.Skip(index+1)).ToArray();
        }
		#endif
    }

    public class DirectedGraphObject<N, L> : DirectedGraphObject<N>
    {
        new public DirectedGraph<N, L> Graph;
        public DirectedGraphObject()
        {
            Graph = new DirectedGraph<N, L>();
        }
    }
}
