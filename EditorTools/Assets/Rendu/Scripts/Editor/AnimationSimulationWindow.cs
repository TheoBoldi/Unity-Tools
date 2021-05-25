using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationSimulationWindow : EditorWindow
{
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    public string[] options;
    public Animator[] animatorsList;

    [MenuItem("Window/Animation Simluation Window")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationSimulationWindow));
    }

    void OnGUI()
    {
        #region animatorlist
        GUILayout.Label("Animators List", EditorStyles.boldLabel);
        animatorsList = GameObject.FindObjectsOfType<Animator>();

        string count = EditorGUILayout.TextField("Number of elements :", animatorsList.Length.ToString());

        for (int i = 0; i < animatorsList.Length; i++)
        {
            animatorsList[i] = (Animator)EditorGUILayout.ObjectField("Element " + i + " :", animatorsList[i], typeof(Animator), true);
        }

        #endregion

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}