using UnityEngine;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private TMP_Text GuitarPicksText;
    //[SerializeField] private Animator GuitarPickAnim;
    private int guitarPicks = 0;

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.gameObject.CompareTag("GuitarPick")) 
        {
            Destroy(collision.gameObject);
            guitarPicks++;
            GuitarPicksText.text = guitarPicks.ToString();
        }
    }

    private void CollectPickAnimation() 
    {
        //GuitarPickAnim.Play("Guitar_Pick_Collect");
    }
}
