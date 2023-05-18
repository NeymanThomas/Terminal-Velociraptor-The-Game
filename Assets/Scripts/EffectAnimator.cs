using UnityEngine;

public class EffectAnimator : MonoBehaviour
{
    [SerializeField] private Animator effects;
    [SerializeField] private SpriteRenderer sr;
    private bool isPositionSet = false;
    private Vector3 position;

    public static EffectAnimator Instance;

    private void Awake() 
    {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);
        }
        else 
        {
            Instance = this;
        }
    }

    private void revert() 
    {
        isPositionSet = false;
        effects.Play("Idle");
    }

    public void setPosition(Vector3 pos) 
    {
        if (!isPositionSet) 
        {
            transform.localPosition = pos;
            isPositionSet = true;
        }
    }

    public void Land(bool isFacingRight) 
    {
        if (isFacingRight) 
        {
            sr.flipX = false;
        }
        else 
        {
            sr.flipX = true;
        }
        effects.Play("Landing_Dust");
    }
}
