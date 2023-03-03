using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject floor;
    [SerializeField] GameObject enemyCube;

    private List<Vector3> randomSpawnPositions = new List<Vector3>();

    [HideInInspector] public int waveCounter;
    [HideInInspector] public float timeUntilNextWave;
    [HideInInspector] public int startingEnemyCount;
    [HideInInspector] public int enemyCount;
    [HideInInspector] public bool spawnedNextWave;
    [HideInInspector] public bool beginCountdownToNextWave;


    private void Awake()
    {
        waveCounter = 1;
        timeUntilNextWave = 3f;
        startingEnemyCount = 5;
        enemyCount = startingEnemyCount;

        spawnedNextWave = false;
        beginCountdownToNextWave = false;
    }

    private void Start()
    {
        SpawnInitialWave();
    }

    private void Update()
    {
        if (enemyCount == 0 && beginCountdownToNextWave == true)    // when no enemies are left, spawn new and bigger wave
        {
            StartCoroutine(SpawnWave());
        }

        if (player.activeSelf == false)     // if player "dies," reload scene
        {
            StartCoroutine(SceneLoadBuffer());
        }
    }

    private void SpawnInitialWave()
    {
        for (int i = 0; i < startingEnemyCount; i++)
        {
            while (randomSpawnPositions.Count < startingEnemyCount)
            {
                Vector3 randomPosition = new Vector3(Random.Range((int)floor.transform.localScale.x / 2 * -1, (int)floor.transform.localScale.x / 2 + 1), floor.transform.position.y + 0.5f, Random.Range((int)floor.transform.localScale.z / 2 * -1, (int)floor.transform.localScale.z / 2 + 1)); ;

                if (randomPosition.x == floor.transform.localScale.x / 2 * -1 || randomPosition.x == floor.transform.localScale.x / 2 || randomPosition.z == floor.transform.localScale.z / 2 * -1 || randomPosition.z == floor.transform.localScale.z / 2 && !randomSpawnPositions.Contains(randomPosition))
                {
                    randomSpawnPositions.Add(randomPosition);
                }
            }

        }

        foreach (Vector3 pos in randomSpawnPositions)
        {
            Instantiate(enemyCube, pos, Quaternion.identity);
        }
    }

    private IEnumerator SpawnWave()
    {
        spawnedNextWave = false;
        beginCountdownToNextWave = false;
        waveCounter++;

        startingEnemyCount = (int)(5 + Mathf.Pow(waveCounter, 2));
        enemyCount = startingEnemyCount;

        randomSpawnPositions.Clear();


        yield return new WaitForSeconds(timeUntilNextWave);

        floor.transform.localScale += new Vector3(2, 0, 2);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < startingEnemyCount; i++)
        {
            while (randomSpawnPositions.Count < startingEnemyCount)
            {
                Vector3 randomPosition = new Vector3(Random.Range((int)floor.transform.localScale.x / 2 * -1, (int)floor.transform.localScale.x / 2 + 1), floor.transform.position.y + 0.75f, Random.Range((int)floor.transform.localScale.z / 2 * -1, (int)floor.transform.localScale.z / 2 + 1)); ;

                if (randomPosition.x == floor.transform.localScale.x / 2 * -1 || randomPosition.x == floor.transform.localScale.x / 2 || randomPosition.z == floor.transform.localScale.z / 2 * -1 || randomPosition.z == floor.transform.localScale.z / 2 && !randomSpawnPositions.Contains(randomPosition))
                {
                    randomSpawnPositions.Add(randomPosition);
                }
            }

        }

        foreach (Vector3 pos in randomSpawnPositions)
        {
            Instantiate(enemyCube, pos, Quaternion.identity);
        }

        spawnedNextWave = true;
    }

    private IEnumerator SceneLoadBuffer()
    {
        startingEnemyCount = 5;

        string currentScene;
        currentScene = SceneManager.GetActiveScene().name;

        yield return new WaitForSeconds(3f);


        SceneManager.LoadScene(currentScene);
    }
}
