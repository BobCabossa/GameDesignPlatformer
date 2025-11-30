using System;
using UnityEngine;

public static class ColliderHelper
{
    public static bool IsIt<T>(Collision2D collision) where T : class
    {
        return IsIt<T>(collision.collider);
    }

    public static bool IsIt<T>(Collider2D collision) where T : class
    {
        Type Ttype = typeof(T);
        if (Ttype == typeof(Player))
        {
            var playerRoot = collision.transform.root;
            return playerRoot.CompareTag("Player");
        }
        else if (Ttype == typeof(Enemy))
        {
            return collision.CompareTag("Enemy");
        }

        Debug.LogWarning("Tried to find a collider type that is not supported (jet)");
        return false;
    }

    public static T GetType<T>(Collision2D collision) where T : class
    {
        return GetType<T>(collision.collider);
    }

    public static T GetType<T>(Collider2D collision) where T : class
    {
        Type Ttype = typeof(T);
        if (Ttype == typeof(Player))
        {
            var playerRoot = collision.transform.root;
            return playerRoot.GetComponent<T>();
        }
        else if (Ttype == typeof(Enemy))
        {
            return collision.GetComponent<T>();
        }

        Debug.LogWarning("Tried to find a collider type that is not supported (jet)");
        return null;
    }
}
