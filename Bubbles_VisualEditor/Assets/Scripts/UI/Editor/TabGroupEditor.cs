using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UIAddons
{
    [CustomEditor(typeof(TabGroup))]
    public class TabGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            if(GUILayout.Button("Create Tab"))
            {
                GameObject go = new GameObject("Tab");
                go.transform.parent = ((TabGroup)target).transform;
                var button = go.AddComponent<TabButton>();
                button.Create();
                ((TabGroup)target).UpdateGroup();


            }
        }
    }

}
