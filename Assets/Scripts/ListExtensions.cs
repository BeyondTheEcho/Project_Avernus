using System;
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

    public static T First<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            throw new InvalidOperationException("The list is empty.");
        }

        return list[0];
    }
}
