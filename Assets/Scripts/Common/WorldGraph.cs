using System;
using System.Collections.Generic;

public static class WorldGraph
{
    public static Dictionary<Type, Object> graph = new Dictionary<Type, Object>();
    public static void Subscribe(Object obj, Type type)
    {
        graph[type] = obj;
    }

    public static Object Retrieve(Type type)
    {
        return graph[type];
    }
}