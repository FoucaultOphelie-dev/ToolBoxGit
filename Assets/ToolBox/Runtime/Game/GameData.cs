using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ToolBox/Game/GameData")]
public class GameData : ScriptableObject
{
    [CustomSliderProperty(1,4)]
    public int nbPlayer = 4;

    public float player1Speed = 15f;
    public float player2Speed = 15f;
    public float player3Speed = 15f;
    public float player4Speed = 15f;
}
