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

    public void TurnTo(bool right)
    {
        Vector3 scale = TextMeshPro.transform.localScale;
        scale.x = right ? 1 : -1;
        TextMeshPro.transform.localScale = scale;
    }
}
