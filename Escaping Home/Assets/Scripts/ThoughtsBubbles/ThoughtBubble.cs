using TMPro;
using UnityEngine;

public class ThoughtBubble : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TextMeshPro;

    [SerializeField]
    private Animator animator;

    public bool WasLastThought(string thought) => TextMeshPro.text == thought;
    public float GetPlayingClipLenght() => animator.GetCurrentAnimatorStateInfo(0).length;
    public void Hide() => animator.Play("Hide");

    public void Show(string thought)
    {
        TextMeshPro.text = thought;
        animator.Play("Show");
    }
}
