using UnityEngine;

public class ThoughtTrigger : MonoBehaviour
{
    public string thought = "No thoughts...";

    public bool removeAfterDelay = false;
    public float delay = 1.0f;

    private float delayTime = 0.0f;
    private bool delayHasRemoved = false;

    private Player Player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!ColliderHelper.IsIt<Player>(collision))
            return;

        Player = ColliderHelper.GetType<Player>(collision);
        Player.ThoughtBubble.Show(thought);

        if (removeAfterDelay)
        {
            delayTime = delay + Player.ThoughtBubble.GetPlayingClipLenght();
            delayHasRemoved = false;
        }
    }

    private void FixedUpdate()
    {
        if (delayHasRemoved)
            return;

        if (Player == null || !removeAfterDelay)
            return;

        delayTime -= Time.deltaTime;
        if (delayTime <= 0)
        {
            Player.ThoughtBubble.Hide();
            delayHasRemoved = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!ColliderHelper.IsIt<Player>(collision))
            return;

        Player = ColliderHelper.GetType<Player>(collision);
        if (Player.ThoughtBubble.WasLastThought(thought))
        {
            Player.ThoughtBubble.Hide();
        }
    }
}
