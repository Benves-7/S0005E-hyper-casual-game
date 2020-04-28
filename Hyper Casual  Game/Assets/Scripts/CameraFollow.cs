using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public bool smoothCameraMovement;
    public float smoothSpeed = 10f;
    public Vector3 offset;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void LateUpdate()
    {
        if (smoothCameraMovement)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
            transform.position = smoothedPosition;
        }
        else
        {
            transform.position = target.position + offset;
        }
        

    }
}
