using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private TMP_Text GuitarPicksText;
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
}
