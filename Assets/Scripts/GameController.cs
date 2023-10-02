using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        instance = this;
        fishPool = new Queue<GameObject>();
        Time.timeScale = 0;

    }
    [SerializeField]
    AudioSource music;
    [SerializeField]
    private TMP_Text scoreUIText, GameStateUIText;
    public float score { get; private set; } = 0;
    public IcebergControls iceberg;
    public GameObject fish;
    public Transform fishSpawner;
    public float fishForce = 5f;
    public static float timeElapsed = 0;
    private float _eventTicker = 5;
    private int shrinkCount = 0;
    private float _shrinkTimer = 15;
    private Queue<GameObject> fishPool;
    public bool gameIsOn = false;

    private void StartGame()
    {
        GameStateUIText.gameObject.SetActive(false);
        score = 0;
        gameIsOn = true;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            music.mute = !music.mute;        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
 
        if (!gameIsOn)
        {
            if(Input.GetKey(KeyCode.Space) && score == 0){
                StartGame();
            }

            if (Input.GetKeyDown(KeyCode.R))
                RestartGame();
            
            return;
        }



        if (_shrinkTimer < 0)
        {
            iceberg.StartShrink();
            _shrinkTimer = 30;
            shrinkCount++;
        }

        if (_eventTicker < 0)
            GameEvent();

        
        score += Time.deltaTime; 

        timeElapsed += Time.deltaTime;
        _shrinkTimer -= Time.deltaTime;
        _eventTicker -= Time.deltaTime;

    }

    private void LateUpdate()
    {
        scoreUIText.text = "Score: " + score.ToString("#.0");
    }


    public void GameEvent()
    {
        LaunchFish();

        _eventTicker = Mathf.Clamp(Random.Range(3.5f, 5f) - shrinkCount, .5f, 10f);
    }


    private void LaunchFish()
    {
        GameObject f;
        if (fishPool.Count > 0) {
            f = fishPool.Dequeue();
            f.SetActive(true);
            f.transform.rotation = Quaternion.identity;
        }
        else
            f = Instantiate(fish, fishSpawner.position, Quaternion.identity);

        Rigidbody rg = f.GetComponent<Rigidbody>();
        rg.useGravity = false;
        rg.AddForce(fishSpawner.up * fishForce, ForceMode.VelocityChange);
        float diskSize = iceberg.transform.localScale.x * 2 * 0.8f;
        Vector3 dir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        dir.Normalize();
        Vector3 target = dir * Random.Range(0f,diskSize);
        f.GetComponent<Fish>().SetTarget(target);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(iceberg.transform.position, iceberg.transform.localScale.x*2 * 0.8f);
    }

    public void AddFishToPool(GameObject fish)
    {
        fishPool.Enqueue(fish);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StopGame()
    {
        gameIsOn = false;
        GameStateUIText.gameObject.SetActive(true);
        GameStateUIText.text = "Game over\n" +
            "You lasted " + score.ToString("#.0") + " seconds before going for a swim with the fish\n" +
            "Press \"R\" to try again or \"Escape\" to exit";

    }
}
