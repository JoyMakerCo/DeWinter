using UnityEngine;

namespace Util
{
	public interface IGraphComponent
	{
		void OnMenu(UnityEditor.GenericMenu menu);
	}
	public interface IGraphNodeComponent : IGraphComponent
	{
		void OnGUI(Vector2 position, bool selected);
		bool Intersect(Vector2 position, Vector2 MousePosition);
	}
	public interface IGraphLinkComponent : IGraphComponent
	{
		void OnGUI(Vector2 tail, Vector2 head, bool selected);
		bool Intersect(Vector2 tail, Vector2 head, Vector2 MousePosition);
	}
}
