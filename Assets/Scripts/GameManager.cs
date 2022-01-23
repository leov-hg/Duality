using DG.Tweening;
using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Separator("References", true)]
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private PlayerRef[] players;
    [SerializeField] private Animator camerasAnim;
    [SerializeField] private Transform focusCamTargetPoint;
    public EndModule endModule;
    [SerializeField] private Transform spawnPointsHolder;
    [SerializeField] private CanvasGroup menuCanvas;

    [Separator("Rounds", true)]
    [SerializeField] private float focusingGoalSpeed = 2;
    [SerializeField] private int poolCount = 100;
    [SerializeField] private float btwRoundDelay = 10;
    [SerializeField] private int enemiesPerRound = 20;

    [Separator("Spawning", true)]
    [SerializeField] private float enemySpawnDelay = .5f;

    private Queue<Enemy> enemies = new Queue<Enemy>();
    private List<Enemy> activeEnemies = new List<Enemy>();

    private int _currentRound = 0;

    public event Action onLevelStart;
    public event Action onLevelComplete;

    private bool _levelStarted;

    private void Awake()
    {
        for (int i = 0; i < poolCount; i++)
        {
            enemies.Enqueue(Instantiate(enemyPrefab, transform));
        }

        
    }

    private void StartLevel()
    {
        camerasAnim.Play("InGame");
        _currentRound = 0;
        onLevelStart?.Invoke();
        StartCoroutine(RoundsLogic());
        _levelStarted = true;
    }

    private void Update()
    {
        if (!_levelStarted && (Input.GetMouseButtonDown(0) || Input.GetButtonDown("Submit")))
        {
            menuCanvas.DOFade(0, 0.5f);
            focusCamTargetPoint.DOMove(endModule.slabPoint.position, focusingGoalSpeed).SetSpeedBased(true)
                .SetLoops(2, LoopType.Yoyo).SetDelay(1).onComplete += StartLevel;
        }
    }

    private IEnumerator RoundsLogic()
    {
        while (true)
        {
            int enemiesToSpawnRemaining = enemiesPerRound;
            while (true)
            {
                Enemy spawnedEnemy = enemies.Dequeue();
                activeEnemies.Add(spawnedEnemy);
                activeEnemies[activeEnemies.Count - 1].Spawn(spawnPointsHolder.GetChild(UnityEngine.Random.Range(0, spawnPointsHolder.childCount)).position);
                spawnedEnemy.onDeath += OnZombieDeath;
                enemiesToSpawnRemaining--;
                if (enemiesToSpawnRemaining == 0)
                {
                    break;
                }
                yield return new WaitForSeconds(enemySpawnDelay);
            }

            yield return new WaitForSeconds(btwRoundDelay);
        }
    }

    public void CompleteLevel()
    {
        onLevelComplete?.Invoke();
        SceneManager.LoadScene(0);
    }

    public void OnZombieDeath(Enemy deadEnemy)
    {
        deadEnemy.onDeath -= OnZombieDeath;
        activeEnemies.Remove(deadEnemy);

        if (activeEnemies.Count == 0)
        {
            onLevelComplete?.Invoke();
        }
    }
}
