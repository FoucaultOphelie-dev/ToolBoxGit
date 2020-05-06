using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CameraPreviewWindow : EditorWindow
{
    private Camera _camera = null;
    private RenderTexture _renderTexture = null;

    [MenuItem("ToolBox/CameraPreview")]

    static void InitWindow()
    {
        EditorWindow window = GetWindow<CameraPreviewWindow>();
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.titleContent = new GUIContent("Camera Preview");
    }

    private void Awake()
    {
        _CreateRenderTexture();
    }

    private void Update()
    {
        if(null == _camera)
        {
            _camera = Camera.main;
        }

        if (null == _camera) return;

        if(null == _renderTexture)
        {
            _CreateRenderTexture();
        }

        RenderTexture tmpCameraTargetTexture = _camera.targetTexture;
        _camera.targetTexture = _renderTexture;
        _camera.Render();
        _camera.targetTexture = tmpCameraTargetTexture;

        if (_renderTexture.width != position.width || _renderTexture.height != position.height)
        {
            _CreateRenderTexture();
        }
    }

    private void OnSelectionChange()
    {
        GameObject selectedGameObject = Selection.activeGameObject;
        if (null != selectedGameObject)
        {
            Camera cameraInsideSelection = selectedGameObject.GetComponentInChildren<Camera>(true);
            if(null != cameraInsideSelection)
            {
                _camera = cameraInsideSelection;
            }
        }
    }

    private void OnGUI()
    {
        if (null != _renderTexture)
        {
            GUI.DrawTexture(new Rect(0, 0, position.width, position.height), _renderTexture);
        }
    }

    private void _CreateRenderTexture()
    {
        _renderTexture = new RenderTexture((int)position.width, (int)position.height, 24, RenderTextureFormat.ARGB32);
    }

}
