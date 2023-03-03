using UnityEngine;

public class DeathRing : MonoBehaviour
{
    private float timeUntilDestroy = 3.5f;

    void Update()
    {
        transform.Rotate(Vector3.forward * -300 * Time.deltaTime);

        transform.localScale += new Vector3(2, 2, 0);       // gotta grow that death ring

        if (transform.localScale.z >= 1f)
        {
            transform.localScale += new Vector3(0, 0, -0.9f);
        }

        timeUntilDestroy -= Time.deltaTime;

        if (timeUntilDestroy <= 0)  
        {
            Destroy(gameObject);
        }
    }
}
