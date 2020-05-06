using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSliderPropertyAttribute : PropertyAttribute
{
    private int _min = 0;
    private int _max = 100;

    public int GetMinValue()
    {
        return _min;
    }
    public int GetMaxValue()
    {
        return _max;
    }
    public CustomSliderPropertyAttribute(int min,int max)
    {
        _min = min;
        _max = max;
    }
}
