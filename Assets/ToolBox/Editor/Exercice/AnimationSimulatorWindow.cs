using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.Animations;

public class AnimationSimulatorWindow : EditorWindow
{
    private Animator[] _animatorComponentsArr = null;
    private string[] _animatorGameObjectsNames = null;
    private int _currentAnimatorIndex = 0;

    private Animator _currentAnimator = null;
    
    private AnimationClip[] _clipsOfCurrentAnimator = null;
    private string[] _clipsNamesOfCurrentAnimator = null;
    private int _currentClipIndex = 0;

    private bool _isPlaying;
    private float _editorLastTime = 0f;

    private Scene _currentScene;

    private float _speed=1f;
    private bool _useSlider;
    private float _valueOfSlider = 0f;

    private float _countDelay = 0f;
    private float _delay = 0f;
    private bool _stop = false;

    [MenuItem("ToolBox/Animation Simulator")]
    static void InitWindow()
    {
        EditorWindow window = GetWindow<AnimationSimulatorWindow>();
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.titleContent = new GUIContent("Animation Siulator");
    }

    private void Awake()
    {
        _currentScene=SceneManager.GetActiveScene();
    }
    private void Update()
    {
        if(_currentScene!= SceneManager.GetActiveScene())
        {
            EditorApplication.update -= _OnEditorUpdate;
            AnimationMode.StopAnimationMode();
            _currentScene = SceneManager.GetActiveScene();
            _animatorComponentsArr = null;
            _animatorGameObjectsNames = null;
            _currentAnimator = null;
            _clipsOfCurrentAnimator = null;
            _clipsNamesOfCurrentAnimator = null;
            _editorLastTime = 0f;
            _currentAnimatorIndex = 0;
            _currentClipIndex = 0;
            _isPlaying = false;
        }
    }
    private void OnGUI()
    {
        if (null == _animatorComponentsArr)
        {
            _animatorComponentsArr = _FindAnimatorComponentsInScene();
        }
        if (null == _animatorGameObjectsNames)
        {
            _animatorGameObjectsNames = _FindAnimatorsGameObjectsNames();
        }
        _currentAnimatorIndex = EditorGUILayout.Popup("Animator", _currentAnimatorIndex, _animatorGameObjectsNames);
        _currentAnimator = _animatorComponentsArr[_currentAnimatorIndex];

        if (_currentAnimator != null)
        {
            if (null == _clipsOfCurrentAnimator)
            {
                _clipsOfCurrentAnimator = _FindAnimClips(_currentAnimator);
            }
            if (null == _clipsNamesOfCurrentAnimator)
            {
                _clipsNamesOfCurrentAnimator = _FindAnimNames(_clipsOfCurrentAnimator);
            }

        }
        GUILayout.Space(10f);
        _currentClipIndex = EditorGUILayout.Popup("Animation", _currentClipIndex, _clipsNamesOfCurrentAnimator);
        ToolBoxEditorGUILayout.BeginBox("Informations");
        GUILayout.Label("Length : " + _clipsOfCurrentAnimator[_currentClipIndex].length);
        if (!_useSlider)
        {
            if (_isPlaying && !_stop)
            {
                GUILayout.Label("Actual Time : " + (Time.realtimeSinceStartup - _editorLastTime) * _speed % _clipsOfCurrentAnimator[_currentClipIndex].length);
            }
            else
            {
                GUILayout.Label("Actual Time : 0");
            }
        }
        else
        {
            GUILayout.Label("Actual Time : "+ _valueOfSlider);
        }
        GUILayout.Label("Looping : " + _clipsOfCurrentAnimator[_currentClipIndex].isLooping);
        ToolBoxEditorGUILayout.EndBox();
        GUILayout.Space(30f);
        _useSlider = GUILayout.Toggle(_useSlider, "Use a slider");
        if (!_useSlider)
        { 
            _speed = EditorGUILayout.Slider("Speed", _speed, 0f, 40f);
            _delay = EditorGUILayout.FloatField("Delay between loops", _delay);
        }
        else
        {
            _valueOfSlider = EditorGUILayout.Slider("Time of Animation", _valueOfSlider, 0f, _clipsOfCurrentAnimator[_currentClipIndex].length);
            if (!_isPlaying)
            {
                EditorGUILayout.HelpBox("You have to play to show the animation",MessageType.Warning);
            }
        }
        GUILayout.Space(30f);
        if (!Application.isPlaying)
        {
            if (_isPlaying)
            {
                if (GUILayout.Button("Stop"))
                {
                    StopAnim();
                }
            }
            else if (GUILayout.Button("Play"))
            {
                PlayAnim();
            }
        }
        else
        {
            if (_isPlaying)
            {
                StopAnim();
            }
        }
        if (GUILayout.Button("Select in scene"))
        {
            Selection.activeGameObject = _currentAnimator.gameObject;
            SceneView.lastActiveSceneView.FrameSelected();
            EditorGUIUtility.PingObject(_currentAnimator.gameObject);
        }

    }

    private Animator[] _FindAnimatorComponentsInScene()
    {
        List<Animator> animatorComponentsList = new List<Animator>();
        foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            animatorComponentsList.AddRange(rootGameObject.GetComponentsInChildren<Animator>());
        }
        return animatorComponentsList.ToArray();
    }

    private string[] _FindAnimatorsGameObjectsNames()
    {
        List<string> animatorNamesList = new List<string>();
        foreach(Animator animator in _animatorComponentsArr)
        {
            animatorNamesList.Add(animator.gameObject.name);
        }
        return animatorNamesList.ToArray();
    }

    private AnimationClip[] _FindAnimClips(Animator animator)
    {
        List<AnimationClip> resultList = new List<AnimationClip>();

        AnimatorController editorController = animator.runtimeAnimatorController as AnimatorController;

        AnimatorControllerLayer controllerLayer = editorController.layers[0];

        foreach (ChildAnimatorState childState in controllerLayer.stateMachine.states)
        {
            AnimationClip animClip = childState.state.motion as AnimationClip;
            if (animClip != null)
            {
                resultList.Add(animClip);
            }
        }

        return resultList.ToArray();
    }

    private string[] _FindAnimNames(AnimationClip[] animClipsArr)
    {
        List<string> resultList = new List<string>();

        foreach (AnimationClip clip in animClipsArr)
        {
            resultList.Add(clip.name);
        }

        return resultList.ToArray();
    }

    private void PlayAnim()
    {
        if (_isPlaying) return;
        _editorLastTime = Time.realtimeSinceStartup;
        EditorApplication.update += _OnEditorUpdate;
        AnimationMode.StartAnimationMode();
        _isPlaying = true;
    }

    private void StopAnim()
    {
        if (!_isPlaying) return;
        EditorApplication.update -= _OnEditorUpdate;
        AnimationMode.StopAnimationMode();
        _isPlaying = false;
    }

    private void _OnEditorUpdate()
    {
        if (_clipsOfCurrentAnimator != null)
        {
            if (!_useSlider)
            {
                DelayToPlay();
                if (!_stop)
                {
                    float animTime = Time.realtimeSinceStartup - _editorLastTime;
                    animTime *= _speed;
                    AnimationClip animClip = _clipsOfCurrentAnimator[_currentClipIndex];
                    if (animTime > animClip.length)
                    {
                        _editorLastTime = Time.realtimeSinceStartup;
                        _stop = true;
                    }
                    
                
                    animTime %= animClip.length;
                    AnimationMode.SampleAnimationClip(_currentAnimator.gameObject, animClip, animTime);
                }
            }
            else
            {
                AnimationClip animClip = _clipsOfCurrentAnimator[_currentClipIndex];
                AnimationMode.SampleAnimationClip(_currentAnimator.gameObject, animClip, _valueOfSlider);
            }
        }
    }

    private void OnDisable()
    {
        if (_isPlaying)
        {
            StopAnim();
        }
    }

    private void DelayToPlay()
    {
        if (_stop)
        {
            if (_countDelay > _delay)
            {
                _stop = false;
                _editorLastTime = Time.realtimeSinceStartup;
            }
            _countDelay = Time.realtimeSinceStartup-_editorLastTime;
        }
        
    }
}
