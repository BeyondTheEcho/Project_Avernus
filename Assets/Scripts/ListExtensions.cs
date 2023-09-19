using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    public static void PopAppend<T>(this List<T> list)
    {
        T firstElement = list[0];
        list.RemoveAt(0);
        list.Add(firstElement);
    }
}
