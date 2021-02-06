using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorDisplays;

namespace EditorDisplays
{
    public static class IntExtensions
    {
        public static void Display(ref this int i, string DisplayName)
        {
            i = EditorGUILayout.IntField(DisplayName, i);
        }
        public static void Display(ref this float i, string DisplayName)
        {
            i = EditorGUILayout.FloatField(DisplayName, i);
        }
    }
   
}

[CustomEditor(typeof(AIController))]
public class AIControllerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AIController t = target as AIController;

        t.MutationChance.Display("Chance To Mutate");
    }
}
