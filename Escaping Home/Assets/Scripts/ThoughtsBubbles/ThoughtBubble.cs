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
}
