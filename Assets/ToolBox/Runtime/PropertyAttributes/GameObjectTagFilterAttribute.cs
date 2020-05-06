using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectTagFilterAttribute : PropertyAttribute
{
    private string _tagFilter = "";

    public string GetTagFilter()
    {
        return _tagFilter;
    }

    public GameObjectTagFilterAttribute(string tagFilter)
    {
        _tagFilter = tagFilter;
    }
}
