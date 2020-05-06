using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public const string GAMEOBJECT_NAME = "Game Manager";

    public GameData gameData;

    private void OnValidate()
    {
        if (gameObject.name != GAMEOBJECT_NAME)
        {
            gameObject.name = GAMEOBJECT_NAME;
        }
    }
}
