using System;
using System.Collections.Generic;

public static class GameEvents
{
    private static Dictionary<string, Action> eventDictionary = new();

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
        if (eventDictionary.ContainsKey(eventName))
            eventDictionary[eventName]?.Invoke();
    }
}