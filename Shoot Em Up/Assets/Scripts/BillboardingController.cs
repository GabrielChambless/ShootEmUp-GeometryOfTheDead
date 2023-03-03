using UnityEngine;

public class BillboardingController : MonoBehaviour
{
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform.position);
        transform.rotation = Camera.main.transform.rotation;
    }
}
