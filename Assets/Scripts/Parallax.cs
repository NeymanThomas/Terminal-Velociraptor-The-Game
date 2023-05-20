using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startPos;
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer sr;
    public float parallaxEffect;

    void Start() 
    {
        startPos = transform.position.x;
        length = sr.bounds.size.x;
    }

    void Update() 
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float distance = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z);

        if (temp > startPos + length) startPos += length;
        else if (temp < startPos - length) startPos -= length;
    }
}
