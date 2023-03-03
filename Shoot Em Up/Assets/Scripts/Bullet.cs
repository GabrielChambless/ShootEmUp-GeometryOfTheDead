using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float timeUntilDestroy = 3f;

    private void Update()
    {
        timeUntilDestroy -= Time.deltaTime;

        if (timeUntilDestroy <= 0)  // destroy bullet after certain amount of time has passed without colliding with something
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }

        if (collision.gameObject.tag == "Wall")
        {
            gameObject.GetComponent<Rigidbody>().useGravity = true;
        }
    }
}
