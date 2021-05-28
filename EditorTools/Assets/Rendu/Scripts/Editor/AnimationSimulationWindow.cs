using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.EditorCoroutines.Editor;

public class AnimationSimulationWindow : EditorWindow
{

    Animator[] animators = new Animator[] { };
    string[] optionsAnimators = new string[] { };
    int indexAnimators = 0;
    int checkAnimatorIndex = 0;

    AnimationClip[] animations = new AnimationClip[] { };
    string[] optionsAnimations = new string[] { };
    int indexAnimations = 0;

    float _lastEditorTime = 0f;
    bool _isSimulatingAnimation = false;

    bool groupEnabled = true;

    bool animationLooping = true;
    bool wasLooping = false;

    bool isPaused = false;

    float animationSpeed = 1f;
    float delayBetweenLoops = 0f;

    float animationSampler = 0f;

    [MenuItem("Window/Animation Simluation Window")]

    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AnimationSimulationWindow));
    }

    private void Update()
    {
        if (indexAnimators != checkAnimatorIndex)
        {
            indexAnimations = 0;
            Selection.activeObject = animators[indexAnimators];
            EditorGUIUtility.PingObject(animators[indexAnimators]);
            SceneView.lastActiveSceneView.FrameSelected();
            checkAnimatorIndex = indexAnimators;
        }
    }

    void OnGUI()
    {
        #region Main objectives
        GUILayout.Label("Animations selection", EditorStyles.boldLabel);
        animators = GameObject.FindObjectsOfType<Animator>();
        optionsAnimators = new string[animators.Length];

        for (int i = 0; i < animators.Length; i++)
        {
            optionsAnimators[i] = animators[i].ToString();
        }

        indexAnimators = EditorGUILayout.Popup("Animators list :",indexAnimators, optionsAnimators);

        animations = new AnimationClip[animators[indexAnimators].runtimeAnimatorController.animationClips.Length];
        optionsAnimations = new string[animations.Length];

        for (int i = 0; i < animators[indexAnimators].runtimeAnimatorController.animationClips.Length; i++)
        {
            animations[i] = animators[indexAnimators].runtimeAnimatorController.animationClips[i];
            optionsAnimations[i] = animations[i].ToString();
        }

        indexAnimations = EditorGUILayout.Popup("Animations list :", indexAnimations, optionsAnimations);

        if (!_isSimulatingAnimation)
        {
            if (GUILayout.Button("Play"))
            {
                StartAnimSimulation();
            }
        }
        else
        {
            if (GUILayout.Button("Stop"))
            {
                animationLooping = false;
                wasLooping = true;
                StopAnimSimulation();
            }

            if (GUILayout.Button("Pause"))
            {
                if (!isPaused)
                    isPaused = true;
                else
                    isPaused = false;
            }
        }
        #endregion

        #region Optional objectives
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        animationLooping = EditorGUILayout.Toggle("Loop animation", animationLooping);
        delayBetweenLoops = EditorGUILayout.FloatField("Delay between loop", delayBetweenLoops);
        animationSpeed = EditorGUILayout.Slider("Animation speed", animationSpeed, 0, 10);
        //animationSampler = EditorGUILayout.Slider("Animation sampler", animationSampler, 0, animations[indexAnimations].length);
        EditorGUILayout.EndToggleGroup();
        #endregion
    }

    private void OnEditorUpdate()
    {
        float animTime = (Time.realtimeSinceStartup - _lastEditorTime) * animationSpeed;

        if (isPaused)
        {
            animTime = _lastEditorTime;
        }

        if (animTime >= animations[indexAnimations].length)
        {
            StopAnimSimulation();
        }
        else
        {
            if (AnimationMode.InAnimationMode())
            {
                AnimationMode.SampleAnimationClip(animators[indexAnimators].gameObject, animations[indexAnimations], animTime);
            }
        }
    }

    public void StartAnimSimulation()
    {
        AnimationMode.StartAnimationMode();
        EditorApplication.update -= OnEditorUpdate;
        EditorApplication.update += OnEditorUpdate;
        _lastEditorTime = Time.realtimeSinceStartup;
        _isSimulatingAnimation = true;
    }

    public void StopAnimSimulation()
    {
        if (animationLooping)
        {
            if(delayBetweenLoops <= 0)
            {
                StartAnimSimulation();
            }
            else
            {
                EditorCoroutineUtility.StartCoroutine(DelayLoop(), this);
            }
        }
        else
        {
            AnimationMode.StopAnimationMode();
            EditorApplication.update -= OnEditorUpdate;
            _isSimulatingAnimation = false;
            isPaused = false;

            if (wasLooping && !animationLooping)
            {
                animationLooping = true;
                wasLooping = false;
            }
        }
    }

    public IEnumerator DelayLoop()
    {
        yield return new EditorWaitForSeconds(delayBetweenLoops);
        StartAnimSimulation();
    }
}