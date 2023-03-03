using UnityEngine;

public class DeathRingPickup : MonoBehaviour
{
    [SerializeField] private GameObject deathRing;
    private GameObject player;

    private float timeUntilDestroy = 15f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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
        if (other.gameObject.tag == "Player")
        {
            Instantiate(deathRing, player.transform.position, deathRing.transform.rotation);

            Destroy(gameObject);
        }
    }
}
