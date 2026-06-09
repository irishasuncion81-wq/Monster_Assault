using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    [Header("Zoom Settings")]
    public float cameraSize = 6f; 

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null) return;

 
        cam.orthographicSize = cameraSize;

 
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}