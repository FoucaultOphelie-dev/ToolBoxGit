using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class ToolBoxWindow : EditorWindow
{
    private int _tabIndex = 0;
    public static string[] tabs = new string[]
    {
        "General",
        "Path2D",
    };

    private Path2D[] _path2DComponentsArr = null;

    [MenuItem("ToolBox/GameBox")]

    static void InitWindow()
    {
        EditorWindow window = GetWindow<ToolBoxWindow>();
        window.autoRepaintOnSceneChange = true;
        window.Show();
        window.titleContent = new GUIContent("GameBox");
    }

    private void OnGUI()
    {
        _tabIndex = GUILayout.Toolbar(_tabIndex, tabs);

        switch (_tabIndex)
        {
            case 0: _GUITabsGeneral(); break;
            case 1: _GUITabsPaths2D(); break;
        }
    }

    private void OnHierarchyChange()
    {
        _path2DComponentsArr = _FindPath2DComponentsInScene();
    }
    private void _GUITabsGeneral()
    {
        GUILayout.Space(50f);
        if (GUILayout.Button("GameManager"))
        {
            _SelectGameManager();
        }

        if (GUILayout.Button("GameData"))
        {
            _SelectGameData();
        }
    }

    private void _GUITabsPaths2D()
    {
        if(null== _path2DComponentsArr)
        {
            _path2DComponentsArr = _FindPath2DComponentsInScene();
        }

        foreach (Path2D path2D in _path2DComponentsArr)
        {
            if (GUILayout.Button(path2D.name))
            {
                Selection.activeGameObject = path2D.gameObject;
                SceneView.lastActiveSceneView.FrameSelected();
                EditorGUIUtility.PingObject(path2D.gameObject);
            }
        }
    }

    private void _SelectGameManager()
    {
        GameManager gameManager = _FindGameManagerInScene();
        if(null != gameManager)
        {
            Selection.activeGameObject = gameManager.gameObject;
            EditorGUIUtility.PingObject(gameManager.gameObject);
        }
    }

    private void _SelectGameData()
    {
        GameManager gameManager = _FindGameManagerInScene();
        if (null != gameManager)
        {
            Selection.activeObject = gameManager.gameData;
            EditorGUIUtility.PingObject(gameManager.gameData);
        }
    }

    private GameManager _FindGameManagerInScene()
    {
        foreach(GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            GameManager gameManager = rootGameObject.GetComponentInChildren<GameManager>();
            if(null != gameManager)
            {
                return gameManager;
            }
        }
        return null;
    }

    private Path2D[] _FindPath2DComponentsInScene()
    {
        List<Path2D> path2DComponentsList = new List<Path2D>();
        foreach (GameObject rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            path2DComponentsList.AddRange(rootGameObject.GetComponentsInChildren<Path2D>());
        }
        return path2DComponentsList.ToArray();
    }
}
