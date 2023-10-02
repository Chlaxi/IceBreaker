using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    private void Awake()
    {
        instance = this;
        fishPool = new Queue<GameObject>();
    }

    public IcebergControls iceberg;
    public GameObject fish;
    public Transform fishSpawner;
    public float fishForce = 5f;
    public static float timeElapsed = 0;
    private float _eventTicker = 10;
    private int shrinkCount = 0;
    private float _shrinkTimer = 45;
    private Queue<GameObject> fishPool;
 
    void Update()
    {
        if (_shrinkTimer < 0)
        {
            iceberg.StartShrink();
            _shrinkTimer = 30;
            shrinkCount++;
        }

        if (_eventTicker < 0)
            GameEvent();
        {
            
        }
        if (Input.GetKeyDown(KeyCode.F))
            LaunchFish();

        timeElapsed += Time.deltaTime;
        _shrinkTimer -= Time.deltaTime;
        _eventTicker -= Time.deltaTime;

    }

    public void GameEvent()
    {
        LaunchFish();

        _eventTicker = Mathf.Clamp(Random.Range(3.5f, 7.5f) - shrinkCount, 1.5f, 10f);
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

}
