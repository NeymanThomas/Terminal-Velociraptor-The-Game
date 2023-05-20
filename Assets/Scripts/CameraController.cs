using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraYOffset;
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
            transform.position = new Vector3(player.position.x, currentPlayerY + cameraYOffset, transform.position.z);
        }
        else if (!isPlayerDucking && currentPlayerY < player.position.y) 
        {
            currentPlayerY += 0.01f;
            transform.position = new Vector3(player.position.x, currentPlayerY + cameraYOffset, transform.position.z);
        }
        else 
        {
            transform.position = new Vector3(player.position.x, player.position.y + cameraYOffset, transform.position.z);
        }
    }

    public void DuckCamera() 
    {
        // this way it doesn't get reassigned over and over
        if (!isPlayerDucking) 
        {
            currentPlayerY = player.position.y;
            isPlayerDucking = true;
        }
    }

    public void RaiseCamera() 
    {
        if (isPlayerDucking) 
        {
            isPlayerDucking = false;
        }
    }
}
