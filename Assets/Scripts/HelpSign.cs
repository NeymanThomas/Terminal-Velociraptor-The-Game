using UnityEngine;

public class HelpSign : MonoBehaviour
{
    [SerializeField] private int SignNumber;
    [SerializeField] private Animator anim;
    private bool isTipActive;

    void Start() 
    {
        anim.Play("Tip_Empty");
        isTipActive = false;
    }

    void Update() 
    {
        if (isTipActive) 
        {
            if (Input.GetButtonDown("Fire1")) 
            {
                anim.Play("Tip_Close");
                isTipActive = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("Player") && !isTipActive) 
        {
            isTipActive = true;
            string tipNum = "Tip_" + SignNumber;
            anim.Play(tipNum);
        }
    }
    
}
