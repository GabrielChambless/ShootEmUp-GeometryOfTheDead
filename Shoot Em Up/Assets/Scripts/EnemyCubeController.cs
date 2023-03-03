using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCubeController : MonoBehaviour
{
    [SerializeField] private Material[] enemyMaterials;
    [SerializeField] private GameObject breakableCube;
    [SerializeField] private GameObject healthPickup;
    [SerializeField] private GameObject deathRingPickup;

    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private float lungeForce;
    [SerializeField] private float attackCooldown;
    private float nextAttackTime = 0f;

    private int health;
    private bool canMove = true;

    private PlayerController playerController;
    private EnemyWaveController enemyWaveController;

    private GameObject player;
    private Rigidbody enemyBody;
    private Renderer enemyRenderer;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        enemyBody = gameObject.GetComponent<Rigidbody>();
        enemyRenderer = gameObject.GetComponent<Renderer>();
    }

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();

        enemyWaveController = GameObject.FindGameObjectWithTag("EnemyWaveController").GetComponent<EnemyWaveController>();

        RandomlyChooseEnemyHealth();
    }

    private void FixedUpdate()
    {
        MoveTowardsPlayer();

        AttackWithinRange();
    }

    private void OnDestroy()
    {
        if (enemyWaveController != null && player.activeSelf == true)
        {
            enemyWaveController.enemyCount--;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            LoseHealthAndDropPickups();
        }

        if (collision.gameObject.tag == "Player")
        {
            DamagePlayer();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeathRing")
        {
            DeathByDeathRing();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            navMeshAgent.enabled = false;
        }
    }

    private void RandomlyChooseEnemyHealth()
    {
        int randomIndex = Random.Range(0, enemyMaterials.Length);

        enemyRenderer.material = enemyMaterials[randomIndex];

        health = randomIndex + 1;
    }

    private void MoveTowardsPlayer()
    {
        if (canMove == true && navMeshAgent.enabled == true)
        {
            navMeshAgent.destination = player.transform.position;
        }
    }

    private void AttackWithinRange()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < attackRange && Time.time > nextAttackTime && canMove == true)
        {
            nextAttackTime = Time.time + attackCooldown;
            StartCoroutine(AttackBuffer(attackCooldown));

            enemyBody.AddForce(transform.up * (lungeForce / 1.5f));
            enemyBody.AddForce(transform.forward * lungeForce);
        }
    }

    private void LoseHealthAndDropPickups()
    {
        health--;

        if (health > 0)
        {
            enemyRenderer.material = enemyMaterials[health - 1];
        }
        else
        {
            if (playerController.currentHealth < playerController.startingHealth && Random.Range(1, 101) <= 10)     // 10% chance to drop health pickup if player's health is below starting health, and if shot with a bullet
            {
                Instantiate(healthPickup, transform.position, Quaternion.identity);
            }
            else if (Random.Range(1, 101) <= 10)         // 10% chance to drop death ring pickup if enemy doesn't drop health, and if shot with a bullet
            {
                Instantiate(deathRingPickup, transform.position, Quaternion.identity);
            }

            GameObject cubeCorpse = Instantiate(breakableCube, transform.position, transform.rotation);

            Rigidbody[] corpsePiecesBodies = cubeCorpse.GetComponentsInChildren<Rigidbody>();

            foreach (Rigidbody piece in corpsePiecesBodies)
            {
                piece.AddForce((transform.forward * -1) * (lungeForce / 3f));
            }

            Destroy(gameObject);
        }
    }

    private void DamagePlayer()
    {
        enemyBody.velocity = Vector3.zero;
        enemyBody.angularVelocity = Vector3.zero;

        if (playerController != null)
        {
            playerController.currentHealth--;

            player.GetComponent<Rigidbody>().AddForce(transform.forward * (lungeForce / 2f));
        }
    }

    private void DeathByDeathRing()
    {
        GameObject cubeCorpse = Instantiate(breakableCube, transform.position, transform.rotation);

        Rigidbody[] corpsePiecesBodies = cubeCorpse.GetComponentsInChildren<Rigidbody>();

        foreach (Rigidbody piece in corpsePiecesBodies)
        {
            piece.AddForce((transform.forward * -1) * (lungeForce / 3f));
        }

        Destroy(gameObject);
    }

    IEnumerator AttackBuffer(float cooldown)
    {
        canMove = false;

        yield return new WaitForSeconds(cooldown);

        canMove = true;
    }
}
