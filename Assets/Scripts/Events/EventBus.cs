using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static Dictionary<Type, Delegate> assignedEvents = new Dictionary<Type, Delegate>();

    public static void Raise(EventData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        Type type = data.GetType();
        if (assignedEvents.TryGetValue(type, out Delegate existingAction)) {
            existingAction?.DynamicInvoke(data);
        }
    }

    public static void Subscribe<T>(Action<T> action) where T : EventData
    {
        Type type = typeof(T);

        if (assignedEvents.ContainsKey(type))
        {
            assignedEvents[type] = Delegate.Combine(assignedEvents[type], action);
        }
        else
        {
            assignedEvents[type] = action;
        }
    }
    
    public static void Unsubscribe<T>(Action<T> action) where T : EventData
    {
        Type type = typeof(T);

        if (assignedEvents.ContainsKey(type))
        {
            assignedEvents[type] = Delegate.Remove(assignedEvents[type], action);
        }
    }

    
}
