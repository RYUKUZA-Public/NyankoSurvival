using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("[ Game Control ]")]
    [SerializeField]
    private bool isLive;
    public bool IsLive => isLive;
    [SerializeField] private float _gameTime;
    public float GameTime => _gameTime;
    [SerializeField] private float _maxGameTime = 3 * 10f;
    public float MaxGameTime => _maxGameTime;

    
    
    [Header("[ Player Info ]")]
    [SerializeField] private int level;
    public int Level => level;
    [SerializeField] private float hp;
    public float Hp { get => hp; set => hp = value; }

    [SerializeField] private float maxHp = 100;
    public float MaxHp => maxHp;
    
    [SerializeField] private int kill;
    public int Kill { get { return kill; } set { kill = value; } }

    [SerializeField] private int exp;
    public int Exp => exp;
    // TODO.
    [SerializeField] private int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public int[] NextExp => nextExp;
    
    public int PlayerId { get; set; }
    
    [Header("[ Game Object ]")]
    [SerializeField] private Player player;
    public Player Player => player;
    [SerializeField] private PoolManager pool;
    public PoolManager Pool => pool;
    [SerializeField] private LevelUpPop levelUpPop;
    public LevelUpPop LevelUpPop => levelUpPop;

    [SerializeField] private Result uiResult;

    [SerializeField] private GameObject enemyCleaner; 

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AudioManager.Instance.PlayBgm(AudioManager.Bgm.Title);
    }

    public void GameStart(int id)
    {
        PlayerId = id;
        hp = maxHp;
        
        player.gameObject.SetActive(true);
        levelUpPop.Select(PlayerId % 2);
        TimeResume();
        
        AudioManager.Instance.PlayBgm(AudioManager.Bgm.Battle);
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Select);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
    
    public void GameVictory()
    {
        StartCoroutine(GameVictoryRoutine());
    }

    private IEnumerator GameVictoryRoutine()
    {
        isLive = false;
        enemyCleaner.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Win();
        TimeStop();
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Win);
        AudioManager.Instance.StopBgm();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    private IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Lose();
        TimeStop();
        
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.Lose);
        AudioManager.Instance.StopBgm();
    }

    private void Update()
    {
        if (!isLive)
            return;
        
        _gameTime += Time.deltaTime;

        if (_gameTime > _maxGameTime)
        {
            _gameTime = _maxGameTime;
            GameVictory();
        }
    }

    public void GetExp()
    {
        if (!isLive)
            return;
        
        exp++;

        if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
        {
            level++;
            exp = 0;
            levelUpPop.Show();
        }
    }

    public void TimeStop()
    {
        isLive = false;
        player.InitTimeStopPlayer();
        Time.timeScale = 0f;
    }
    
    public void TimeResume()
    {
        isLive = true;
        Time.timeScale = 1f;
    }
}
