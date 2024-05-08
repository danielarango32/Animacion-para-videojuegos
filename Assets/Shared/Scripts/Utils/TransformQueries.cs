using System.Linq;
using UnityEngine;

public static class TransformQueries
{
    public static Transform Q(this Transform t, string name)
    {
        return t.GetComponentsInChildren<Transform>().First(x => x.gameObject.name == name);
    }

    public static TComp Q<TComp>(this Transform t, string name) where TComp : Component
    {
        return t.GetComponentsInChildren<TComp>().First(x => x.gameObject.name == name);
    }
}
