using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testProperty : MonoBehaviour
{
    [CustomSliderProperty(0, 100)]
    public int number;


    [GameObjectTagFilter("MainCamera")]
    public GameObject testTagCamera;
}
