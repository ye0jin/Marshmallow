using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private string keyName = "BestScore";
    private float bestScore = float.MaxValue;

    public static GameManager Instance;

    private bool isBoss = false;
    public bool Boss => isBoss;

    private bool gameOver = false;

    [SerializeField] private GameObject enemyPref;
    [SerializeField] private PlayerHealth deadSection;

    [SerializeField] private GameObject boss;

    [SerializeField] private Transform spawnPointParent;
    private List<Transform> spawnPoint;

    private float maxSpawnDelay = 8f;

    private float currentPlayingTime = 0f;
    public float CurrentPlayingTime => currentPlayingTime;

    private float currentEndTime = 0f; // 클리어 걸린 시간
    public float CurrentEndTime => currentEndTime;
    private float maxEndTime = 0f; // 최대로 빨리 끝난 시간

    [SerializeField] private StageSO stage;
    private int currentStage = 0;
    public int CurrentStage => currentStage;

    private void Awake()
    {
        bestScore = PlayerPrefs.GetFloat(keyName, float.MaxValue);
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager!");
        }
        Instance = this;

        spawnPoint = new List<Transform>();
        spawnPointParent.GetComponentsInChildren<Transform>(spawnPoint);
        spawnPoint.RemoveAt(0);

        deadSection = deadSection.GetComponent<PlayerHealth>();
        boss.SetActive(false);
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        float currentTime = 0;

        while(true)
        {
            currentTime += Time.deltaTime;

            if(currentTime>=3f)
            {
                currentTime = 0;
                int idx = Random.Range(0, spawnPoint.Count);
                int cnt = Random.Range(1, 4);

                for (int i = 0; i < cnt; i++) 
                {
                    GameObject enemy = Instantiate(enemyPref);
                    Vector2 posOffset = Random.insideUnitCircle * 10;
                    posOffset.y = 0;

                    enemy.transform.position = spawnPoint[idx].position + (Vector3)posOffset;
                }

                float delay = Random.Range(3, maxSpawnDelay);
                yield return new WaitForSeconds(delay);
            }

            yield return null;
        }
    }

    private void Update()
    {
        if (gameOver) return;
        if(deadSection.PlayerDead)
        {
            StopAllCoroutines();
            gameOver = true;
            return;
        }

        if(UIManager.Instance.CurrentEnemyKill >= stage.Stage[currentStage].EnemyCount && !isBoss) //  후에 스테이지별로 다르게, 보스전
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject obj in objectsWithTag)
            {
                Destroy(obj);
            }

            //Debug.Log("Boss!!!!!!!!!!!!11");
            isBoss = true;
            boss.SetActive(true);
            StopAllCoroutines();
        }

        currentPlayingTime += Time.deltaTime;
    }

    public void SaveGameOverTime()
    {
        currentEndTime = currentPlayingTime;
        if (currentEndTime < bestScore) 
        {
            PlayerPrefs.SetFloat(keyName, currentEndTime);
        }
    }
}
