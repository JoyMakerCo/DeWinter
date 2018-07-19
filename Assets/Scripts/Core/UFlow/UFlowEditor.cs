#if (UNITY_EDITOR)

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Util;

namespace UFlow
{
    public class UFlowEditor : GraphEditorWindow
    {
        Dictionary<string, UStateNode> _nodeTypes;
        Dictionary<string, UGraphLink> _linkTypes;

        void Awake()
        {
            MonoScript[] scripts = Resources.FindObjectsOfTypeAll<MonoScript>();
            Type shader = typeof(Shader);
            Type node = typeof(UState);
            Type link = typeof(ULink);
            _nodeTypes = new Dictionary<string, UStateNode>();
            _linkTypes = new Dictionary<string, UGraphLink>();
            foreach(MonoScript script in scripts)
            {
                if (script.GetType() != shader
                    && script.GetClass() != null
                    && script.GetClass().Namespace != "UFlow")
                    if (script.GetClass().IsSubclassOf(node))
                    {
                        _nodeTypes.Add(script.GetClass().ToString(), null); // TODO
                    }
                    else if (script.GetClass().IsSubclassOf(link))
                    {
                        _linkTypes.Add(script.GetClass().ToString(), null); // TODO
                    }
            }
        }
    }
}

#endif
