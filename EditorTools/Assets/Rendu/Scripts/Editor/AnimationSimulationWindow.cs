using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationSimulationWindow : EditorWindow
{
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    public Animator[] animatorsList;
    public AnimationClip[] animations;

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

        for(int i = 0; i < animations.Length; i++)
        {
            for(int j = 0; j < animatorsList[i].runtimeAnimatorController.animationClips.Length; j++)
            {
                animations[i] = animatorsList[i].runtimeAnimatorController.animationClips[j];
            }
            animations[i] = (AnimationClip)EditorGUILayout.ObjectField("Element " + i + " :", animations[i], typeof(AnimationClip), true);
        }

        #endregion

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}