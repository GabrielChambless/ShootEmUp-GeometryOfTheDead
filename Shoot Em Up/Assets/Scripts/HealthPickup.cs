using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    private PlayerController playerController;

    private float timeUntilDestroy = 15f;

    private void Awake()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 60 * Time.deltaTime);

        timeUntilDestroy -= Time.deltaTime;

        if (timeUntilDestroy <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && playerController.currentHealth < playerController.startingHealth)
        {
            if (playerController.currentHealth >= 5)
            {
                playerController.currentHealth = playerController.startingHealth;
            }
            else if (playerController.currentHealth < 5)
            {
                playerController.currentHealth += 2;
            }

            Destroy(gameObject);
        }
    }
}
