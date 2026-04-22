using System;
using System.Collections.Generic;

public static class GameEvents
{
    private static Dictionary<string, Action> eventDictionary = new();
    private static HashSet<string> currentlyTriggering = new();

    public static void Subscribe(string eventName, Action listener)
    {
        if (!eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] = listener;
        else
            eventDictionary[eventName] += listener;
    }

    public static void Unsubscribe(string eventName, Action listener)
    {
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName] -= listener;
    }

    public static void Trigger(string eventName)
    {
        if (currentlyTriggering.Contains(eventName)) return;

        currentlyTriggering.Add(eventName);
        try
        {
            if (eventDictionary.ContainsKey(eventName))
                eventDictionary[eventName]?.Invoke();
        }
        finally
        {
            currentlyTriggering.Remove(eventName);
        }
    }
}