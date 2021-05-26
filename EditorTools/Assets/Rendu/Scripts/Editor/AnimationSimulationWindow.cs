using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AnimationSimulationWindow : EditorWindow
{
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    public Animator[] animators = new Animator[] { };
    public string[] optionsAnimators = new string[] { };
    public int indexAnimators = 0;

    public AnimationClip[] animations = new AnimationClip[] { };
    public string[] optionsAnimations = new string[] { };
    public int indexAnimations = 0;

    [MenuItem("Window/Animation Simluation Window")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationSimulationWindow));
    }

    private void Awake()
    {
        
    }

    void OnGUI()
    {
        #region animatorlist
        GUILayout.Label("Animators List", EditorStyles.boldLabel);
        animators = GameObject.FindObjectsOfType<Animator>();
        optionsAnimators = new string[animators.Length];
        animations = new AnimationClip[animators[indexAnimators].runtimeAnimatorController.animationClips.Length];
        optionsAnimations = new string[animations.Length];

        for (int i = 0; i < animators.Length; i++)
        {
            optionsAnimators[i] = animators[i].ToString();
        }

        indexAnimators = EditorGUILayout.Popup(indexAnimators, optionsAnimators);

        for(int i = 0; i < animators[indexAnimators].runtimeAnimatorController.animationClips.Length; i++)
        {
            animations[i] = animators[indexAnimators].runtimeAnimatorController.animationClips[i];
            optionsAnimations[i] = animations[i].ToString();
        }

        indexAnimations = EditorGUILayout.Popup(indexAnimations, optionsAnimations);

        #endregion

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}