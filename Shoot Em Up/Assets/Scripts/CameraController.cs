using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private GameObject player;

    private float smoothSpeed = 4f;     // for lerp
    private Vector3 offset = new Vector3(20f, 16f, -20f);

    private float turnSpeed = 1.5f;    // for rotation


    private void LateUpdate()
    {
        if (Input.GetKey(KeyCode.Q))    // rotate camera clockwise
        {
            offset = Quaternion.AngleAxis(-1 * turnSpeed, Vector3.up) * offset;

            transform.position = target.position + offset;
            transform.LookAt(target.position);
        }
        else if (Input.GetKey(KeyCode.E))   // rotate camera counter-clockwise
        {
            offset = Quaternion.AngleAxis(1 * turnSpeed, Vector3.up) * offset;

            transform.position = target.position + offset;
            transform.LookAt(target.position);
        }
        else   // no rotation
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            transform.position = smoothedPosition;
        }

    }
}
