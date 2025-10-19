using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugLogger
{
    private static Dictionary<string, bool> categoryStates = new Dictionary<string, bool>();

    public static void Enable(string category) => categoryStates[category] = true;
    public static void Disable(string category) => categoryStates[category] = false;
    public static void Toggle(string category)
    {
        if (!categoryStates.ContainsKey(category))
            categoryStates[category] = true;
        else
            categoryStates[category] = !categoryStates[category];
    }

    public static bool IsEnabled(string category)
    {
        return categoryStates.ContainsKey(category) && categoryStates[category];
    }

    public static void Log(string category, string message)
    {
        if (IsEnabled(category))
            Debug.Log($"[{category}] {message}");
    }

    public static void Warning(string category, string message)
    {
        if (IsEnabled(category))
            Debug.LogWarning($"[{category}] {message}");
    }

    public static void Error(string category, string message)
    {
        if (IsEnabled(category))
            Debug.LogError($"[{category}] {message}");
    }
}