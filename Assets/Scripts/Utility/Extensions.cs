using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    /// <summary>
    /// Generates a new Vector3 with the X, Y, and Z components
    /// rounded to the specified precision
    /// </summary>
    /// <param name="precision">how many decimal places to round the values to</param>
    /// <returns>new Vector3 with rounded components</returns>
    public static Vector3 RoundedComponents(this Vector3 v, int precision = 0)
    {
        float multiplier = Mathf.Pow(10, precision);
        return new Vector3( Mathf.Round(v.x * multiplier) / multiplier,
                            Mathf.Round(v.y * multiplier) / multiplier,
                            Mathf.Round(v.z * multiplier) / multiplier);
    }

    /// <summary>
    /// Gets the requested component, creating it if it doesn't already exist
    /// </summary>
    /// <typeparam name="T">component type</typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// Gets the requested component, logging an error if it doesn't exist
    /// </summary>
    /// <typeparam name="T">component type</typeparam>
    /// <returns></returns>
    public static T GetRequiredComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component != null)
            return component;

        Debug.LogErrorFormat(gameObject, "Failed to find component of type {0}", typeof(T));
        Debug.Break();
        return null;
    }

    /// <summary>
    /// Gets the requested component, logging an error if it doesn't exist
    /// </summary>
    /// <typeparam name="T">component type</typeparam>
    /// <returns></returns>
    public static T GetRequiredComponent<T>(this Component component) where T : Component => component.gameObject.GetRequiredComponent<T>();

    /// <summary>
    /// Adds an item to the list only if it is not already there
    /// </summary>
    /// <param name="value">value to add</param>
    /// <returns></returns>
    public static bool SafeAdd<T>(this List<T> list, T value)
    {
        if (list.Contains(value))
            return false;

        list.Add(value);
        return true;
    }

    /// <summary>
    /// Adds an item to the array only if it is not already there
    /// </summary>
    /// <param name="value">value to add</param>
    /// <returns></returns>
    public static bool SafeAdd<T>(this T[] arr, T value)
    {
        for (int index = 0; index < arr.Length; ++index)
        {
            if (arr[index].Equals(value))
                return false;
        }
        Array.Resize(ref arr, arr.Length + 1);
        arr[arr.Length - 1] = value;
        return true;
    }

    /// <summary>
    /// Adds an item to the dictionary only if it is not already there
    /// </summary>
    /// <param name="key">key for the new value</param>
    /// <param name="value">value to add</param>
    /// <returns></returns>
    public static bool SafeAdd<K, V>(this Dictionary<K, V> dict, K key, V value)
    {
        if (dict.ContainsKey(key))
            return false;

        dict.Add(key, value);
        return true;
    }

    /// <summary>
    /// Removes the specified item from the list. Optimized when removing early elements.
    /// Does not preserve element order.
    /// </summary>
    /// <param name="item">item to remove</param>
    /// <returns></returns>
    public static bool QuickRemove<T>(this List<T> list, T item)
    {
        int index = list.IndexOf(item);
        if (index < 0)
            return false;

        list.QuickRemoveAt(index);
        return true;
    }

    /// <summary>
    /// Removes the specified item from the array. Optimized when removing early elements.
    /// Does not preserve element order.
    /// </summary>
    /// <param name="item">item to remove</param>
    /// <returns></returns>
    public static bool QuickRemove<T>(this T[] arr, T item)
    {
        for (int index = 0; index < arr.Length; ++index)
        {
            if (arr[index].Equals(item))
            {
                arr.QuickRemoveAt(index);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Removes the item at <i>index</i> from the list. Optimized when removing early elements.
    /// Does not preserve element order.
    /// </summary>
    /// <param name="index">element index to remove</param>
    public static void QuickRemoveAt<T>(this List<T> list, int index)
    {
        list[index] = list[list.Count - 1];
        list.RemoveAt(list.Count - 1);
    }

    /// <summary>
    /// Removes the item at <i>index</i> from the list. Optimized when removing early elements.
    /// Does not preserve element order.
    /// </summary>
    /// <param name="index">element index to remove</param>
    public static void QuickRemoveAt<T>(this T[] arr, int index)
    {
        arr[index] = arr[arr.Length - 1];
        arr[arr.Length - 1] = default(T);
        Array.Resize<T>(ref arr, arr.Length - 1);
    }
}
