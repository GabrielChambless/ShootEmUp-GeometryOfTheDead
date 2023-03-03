using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceController : MonoBehaviour
{
    [SerializeField] GameObject menuCanvas;
    [SerializeField] GameObject inGameCanvas;
    [SerializeField] GameObject controlsUI;
    [SerializeField] GameObject heartToggleImage;
    [SerializeField] GameObject waveCounterObject;
    [SerializeField] GameObject remainingEnemiesObject;
    [SerializeField] GameObject timeTextObject;
    [SerializeField] EnemyWaveController enemyWaveController;
    [SerializeField] GameObject player;

    private Text waveCounterText;
    private Text remainingEnemiesText;
    private Text timeText;

    public static bool gameIsPaused = true;     // static

    private float timeRemaining = 0;
    private bool timeIsRunning = false;


    private void OnEnable()
    {
        gameIsPaused = true;

        Time.timeScale = 0;
    }

    private void Start()
    {
        waveCounterText = waveCounterObject.GetComponent<Text>();
        remainingEnemiesText = remainingEnemiesObject.GetComponent<Text>();
        timeText = timeTextObject.GetComponent<Text>();
    }

    void Update()
    {
        DisplayRemainingEnemiesIfEnemies();

        CheckToDisableRemainingEnemies();

        CheckToEnableRemainingEnemies();

        CountdownTimerUntilNextWave();

        PauseGame();
    }

    private void CheckToDisableRemainingEnemies()
    {
        if (enemyWaveController.enemyCount == 0 && timeRemaining <= 0f)     // use timeRemaing as a condition instead of creating a new variable, since it will always be zero before activating the timer
        {

            remainingEnemiesObject.SetActive(false);

            StartCoroutine(FadeBuffer());   // start UI fade animation
        }
    }

    private void CountdownTimerUntilNextWave()
    {
        if (timeIsRunning == true)      // start countdown timer until next wave
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timeIsRunning = false;
            }

            DisplayTime(timeRemaining);
        }
        else if (timeIsRunning == false)
        {
            timeTextObject.SetActive(false);
        }
    }

    private void DisplayRemainingEnemiesIfEnemies()
    {
        if (remainingEnemiesObject.activeSelf == true && enemyWaveController.enemyCount > 0 && player.activeSelf == true)
        {
            remainingEnemiesText.text = "Zomb-Cubes: " + enemyWaveController.enemyCount + "/" + enemyWaveController.startingEnemyCount;
        }
        else
        {
            remainingEnemiesObject.SetActive(false);
        }
    }

    private void CheckToEnableRemainingEnemies()
    {
        if (enemyWaveController.spawnedNextWave == true && menuCanvas.activeSelf == false)      // if new wave has just spawned and not paused, display remaining zomb-cubes
        {
            enemyWaveController.spawnedNextWave = false;
            remainingEnemiesObject.SetActive(true);
        }
    }

    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))       
        {
            if (gameIsPaused == false)
            {
                gameIsPaused = true;
                Time.timeScale = 0;

                inGameCanvas.SetActive(false);
                menuCanvas.SetActive(true);
            }
            else
            {
                gameIsPaused = false;
                Time.timeScale = 1;

                menuCanvas.SetActive(false);
                controlsUI.SetActive(false);
                inGameCanvas.SetActive(true);
            }
        }
    }

public void SlayButton()
    {
        gameIsPaused = false;
        Time.timeScale = 1;

        menuCanvas.SetActive(false);
        controlsUI.SetActive(false);
        inGameCanvas.SetActive(true);
    }

    public void ControlsButton()
    {
        menuCanvas.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void ControlsBackButton()
    {
        controlsUI.SetActive(false);
        menuCanvas.SetActive(true);
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void TogglePlayerControls()
    {
        if (PlayerController.globalControls == true)
        {
            PlayerController.globalControls = false;
            heartToggleImage.SetActive(true);
        }
        else
        {
            PlayerController.globalControls = true;
            heartToggleImage.SetActive(false);
        }
    }


    private void DisplayTime(float timeToDisplay)
    {
        timeTextObject.SetActive(true);

        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }


    private IEnumerator FadeBuffer()
    {
        timeRemaining = 4f;
        enemyWaveController.timeUntilNextWave = timeRemaining - 1;      // -1 in order to show the countdown starting from 0:03 instead of 0:002 if timeRemaining was equal to 3f
        waveCounterText.text = "WAVE: " + (enemyWaveController.waveCounter + 1);
        waveCounterObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        waveCounterObject.SetActive(false);

        timeIsRunning = true;
        enemyWaveController.beginCountdownToNextWave = true;

    }
}
