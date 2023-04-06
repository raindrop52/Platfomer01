using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tween_Item))]
public class TweenEditButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Tween_Item uiTween = (Tween_Item)target;

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if(GUILayout.Button("¿Ãµø", GUILayout.Width(120), GUILayout.Height(30)))
        {
            uiTween.Move();
        }
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }
}
