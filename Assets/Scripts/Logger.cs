using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger
{
    private readonly string category;

    public Logger(string category)
    {
        this.category = category;
    }

    public void Log(string message)
    {
        DebugLogger.Log(category, message);
    }

    public void Warning(string message)
    {
        DebugLogger.Warning(category, message);
    }

    public void Error(string message)
    {
        DebugLogger.Error(category, message);
    }

    public void Enable() => DebugLogger.Enable(category);
    public void Disable() => DebugLogger.Disable(category);
    public void Toggle() => DebugLogger.Toggle(category);
    public bool IsEnabled => DebugLogger.IsEnabled(category);
}
