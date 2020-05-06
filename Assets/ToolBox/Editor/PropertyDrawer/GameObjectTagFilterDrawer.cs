using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using System;

[CustomPropertyDrawer(typeof(GameObjectTagFilterAttribute))]
public class GameObjectTagFilterDrawer : PropertyDrawer
{
    private GameObject[] _gameObjectsArr = null;
    private string[] _gameObjectsNameArr = null;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GameObjectTagFilterAttribute tagFilterAttribute = attribute as GameObjectTagFilterAttribute;

        if(null == _gameObjectsArr)
        {
            _gameObjectsArr = FindGameObjectWithTagInScene(tagFilterAttribute.GetTagFilter());
        }

        if(null == _gameObjectsNameArr)
        {
            List<string> namesList = new List<string>();
            foreach(GameObject gameObject in _gameObjectsArr)
            {
                namesList.Add(gameObject.name);
            }
            _gameObjectsNameArr = namesList.ToArray();
        }
        GameObject currentGameObject = property.objectReferenceValue as GameObject;
        int currentIndex = Array.IndexOf(_gameObjectsArr, currentGameObject);
        if (currentIndex < 0)
        {
            currentIndex = 0;
        }
        currentIndex = EditorGUI.Popup(position,label.text, currentIndex, _gameObjectsNameArr);
        property.objectReferenceValue = _gameObjectsArr[currentIndex];
    }
    private GameObject[] FindGameObjectWithTagInScene(string tag)
    {
        List<GameObject> resultList = new List<GameObject>();
        Scene activeScene = SceneManager.GetActiveScene();
        foreach(GameObject gameObject in activeScene.GetRootGameObjects())
        {
            foreach (Transform childTransform in gameObject.GetComponentsInChildren<Transform>())
            {
                if (childTransform.gameObject.CompareTag(tag))
                {
                    resultList.Add(childTransform.gameObject);
                }
            }
        }
        return resultList.ToArray();
    }
}
