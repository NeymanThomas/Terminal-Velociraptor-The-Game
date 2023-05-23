using UnityEngine;

public class LevelOneHandler : MonoBehaviour
{
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
}
