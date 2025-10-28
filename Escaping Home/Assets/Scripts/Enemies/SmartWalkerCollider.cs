using System.Collections.Generic;
using UnityEngine;

public class SmartWalkerCollider : MonoBehaviour
{
    public SmartWalker SmartWalker;

    private List<GameObject> grounds = new();


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            grounds.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            grounds.Remove(collision.gameObject);
            if (grounds.Count == 0)
            {
                SmartWalker.TurnAround();
            }
        }
    }
}

