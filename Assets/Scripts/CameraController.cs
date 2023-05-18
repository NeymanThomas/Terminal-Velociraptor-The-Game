using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private bool isPlayerDucking;
    private float currentPlayerY;

    public static CameraController Instance;

    public bool IsPlayerDucking 
    {
        get 
        {
            return isPlayerDucking;
        }
    }

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

    void Update()
    {
        if (isPlayerDucking) 
        {
            if (currentPlayerY > player.position.y - 2) 
            {
                currentPlayerY -= 0.005f;
            }
            transform.position = new Vector3(player.position.x, currentPlayerY + 1, transform.position.z);
        }
        else 
        {
            transform.position = new Vector3(player.position.x, player.position.y + 1, transform.position.z);
        }
    }

    public void DuckCamera(bool isDucking) 
    {
        if (!isPlayerDucking) 
        {
            currentPlayerY = player.position.y;
        }
        isPlayerDucking = isDucking;
    }
}
