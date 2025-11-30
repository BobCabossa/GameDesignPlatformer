using TMPro;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextMeshPro;

    [SerializeField]
    private Animator animator;

    public void Show(string thought)
    {
        TextMeshPro.text = thought;
        animator.Play("Show");
    }

    public void Hide()
    {
        animator.Play("Hide");
    }

    public bool WasLastThought(string thought)
    {
        return TextMeshPro.text == thought;
    }

    public float GetPlayingClipLenght()
    {
        return animator.GetCurrentAnimatorStateInfo(0).length;
    }
}
