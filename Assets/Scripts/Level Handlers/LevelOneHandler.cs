using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelOneHandler : MonoBehaviour
{
    [SerializeField] private Image dialogueBox;
    [SerializeField] private Image dialogueFace_1;
    [SerializeField] private Image dialogueFace_2;
    [SerializeField] private TMP_Text dialogue;
    [SerializeField] private GameObject dialogueHolder;

    public static LevelOneHandler Instance;

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

    void Start()
    {
        dialogueHolder.gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
