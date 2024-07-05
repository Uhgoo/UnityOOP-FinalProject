using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject[] crabList;
    [SerializeField] private TextMeshProUGUI midText;

    [SerializeField] private Button restartButton;

    [SerializeField] private TextMeshProUGUI timer;

    [SerializeField] private PhysicMaterial icy;
    private int score = 0;

    private int normalRoundNum = 0;

    private GameObject player;
    [SerializeField] private GameObject[] foodList;
    [SerializeField] public GameObject[] explosionList;
    [SerializeField] private GameObject particlePouf;
    private PlayerMovement playerMovement;

    public bool isGameOn { get; private set; }
    private float timeBetweenSpawn;

    private float timeWait = 2.5f;
    private WaitForSeconds waitFor;

    [SerializeField] private GameObject powerUp;
    private Vector3[] pos = {
        new Vector3(-15, 4.5f, 0),
        new Vector3(10, 8.5f, 0),
        new Vector3(-15.5f, -4.75f, 0),
        new Vector3(-2.25f, 6, 0),
        new Vector3(0, -2.5f, 0),
        new Vector3(16.75f, 3, 0)
    };

    // Start is called before the first frame update
    void Awake()
    {
        InstantiatePlayer();
        playerMovement = player.GetComponent<PlayerMovement>();
        waitFor = new WaitForSeconds(timeWait);

        midText.enabled = false;
        restartButton.gameObject.SetActive(false);
        isGameOn = true;
        
        StartCoroutine(PlayGame());
        StartCoroutine(Timer());
    }

    private IEnumerator PlayGame()
    {
        // Countdown
        StartCoroutine(ShowText("3"));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowText("2"));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowText("1"));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowText("GO!"));
        WaitForSeconds waitForNumSec = new WaitForSeconds(1);

        // Start Spawning

        while (isGameOn)
        {
            yield return waitForNumSec;

            switch (normalRoundNum++)
            {
                case 0:
                    StartCoroutine(SpawnWalkingCrabs(2));
                    waitForNumSec = new WaitForSeconds(10);
                    break;
                case 1:
                    StartCoroutine(SpawnNinjaCrabs(1));
                    StartCoroutine(SpawnWalkingCrabs(2));
                    break;
                case 2:
                    StartCoroutine(SpawnNinjaCrabs(2));
                    StartCoroutine(SpawnWalkingCrabs(3));
                    StartCoroutine(SpawnPowerUp(4 * timeWait));
                    break;
                case 3:
                    ActivateBigCrab();
                    StartCoroutine(SpawnWalkingCrabs(4));
                    break;
                case 4:
                    StartCoroutine(SpawnNinjaCrabs(3));
                    break;
                default:
                    StartCoroutine(SpawnWalkingCrabs(4));
                    StartCoroutine(SpawnNinjaCrabs(2));
                    if (timeWait > 1.5f)
                    {
                        timeWait -= 0.1f;
                        waitFor = new WaitForSeconds(timeWait);
                    }
                    waitForNumSec = new WaitForSeconds(4 * timeWait);

                    StartCoroutine(SpawnPowerUp(4 * timeWait));
                    break;
            }
        }
    }

    private void InstantiatePlayer()
    {
        player = Instantiate(foodList[MainManager.Instance.selectedFood], new Vector3(0, -6.5f, 0), Quaternion.identity);
        player.AddComponent<MeshCollider>().convex = true;
        player.GetComponent<MeshCollider>().material = icy;
        player.AddComponent<PlayerMovement>().poufParticle = particlePouf;
    }

    private IEnumerator SpawnWalkingCrabs(int num)
    {
        for (int i = 0; i < num; i++)
        {
            Instantiate(crabList[0], new Vector3(-22, -6.5f, 0), Quaternion.Euler(0, 180, 0));
            yield return waitFor;
        }
    }

    private IEnumerator SpawnNinjaCrabs(int num)
    {
        Vector3 spawnPos = Vector3.zero;

        for (int i = 0; i < num; i++)
        {
            switch (Random.Range(0, 3))
            {
                case 0:
                    spawnPos = new Vector3(8.25f, 12, 0);
                    break;
                case 1:
                    spawnPos = new Vector3(20, 4, 0);
                    break;
                case 2:
                    spawnPos = new Vector3(-16.5f, 12, 0);
                    break;
            }

            Instantiate(crabList[1], spawnPos, Quaternion.Euler(0, 180, 0));
            yield return waitFor;
        }
    }

    private void ActivateBigCrab()
    {
        GameObject.Find("GiantCrab").GetComponent<BigCrab>().isActive = true;
    }

    private IEnumerator ShowText(string toDisplay)
    {
        float maxSize = 70f;
        
        midText.text = toDisplay;
        midText.enabled = true;
        midText.fontSize = maxSize;

        float time = 1f;

        while (time > 0)
        {
            midText.fontSize = time * maxSize;
            time -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator ShowText(string toDisplay, float time)
    {
        midText.text = toDisplay;
        midText.enabled = true;
        midText.fontSize = 15f;

        while (time > 0)
        {
            midText.fontSize = time * 15f;
            time -= Time.deltaTime;
            yield return null;
        }
    }

    public void GameOver()
    {
        StartCoroutine(ShowText("You got eaten HAHA", 5f));
        restartButton.gameObject.SetActive(true);
        isGameOn = false;
        player.GetComponent<PlayerMovement>().Explode();
    }

    private IEnumerator Timer()
    {
        WaitForSeconds w = new WaitForSeconds(1);
        yield return new WaitForSeconds(3);
        while (isGameOn)
        {
            yield return w;
            timer.text = "Time: " + ++score;
        }
        if (MainManager.Instance.topScore <= score)
        {
            MainManager.Instance.topScore = score;
            MainManager.Instance.topPlayerName = MainManager.Instance.playerName;
            MainManager.Instance.SaveGameData();
        }
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
    }

    private IEnumerator SpawnPowerUp(float timeAlive)
    {
        Vector3 position = pos[Random.Range(0, pos.Length)];

        GameObject power = Instantiate(powerUp, position, Quaternion.identity);
        
        Vector3 finalSize = power.transform.localScale;
        float time = 0;

        while (time < 1f)
        {
            if (power != null)
                power.transform.localScale = time * finalSize;
            else break;

            time += Time.deltaTime;
            yield return null;
        }

        if (power != null)
            power.transform.localScale = finalSize;

        yield return new WaitForSeconds(timeAlive);

        time /= 2;

        while (time > 0f)
        {
            if (power != null)
                power.transform.localScale = time * finalSize;
            else break;

            time -= Time.deltaTime;
            yield return null;
        }

        if (power != null)
            Destroy(power);
    }
}
