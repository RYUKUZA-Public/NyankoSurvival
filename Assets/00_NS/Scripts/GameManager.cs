using System;
using UnityEngine;

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
    [SerializeField] private int hp;
    public int Hp { get => hp; set => hp = value; }

    [SerializeField] private int maxHp = 100;
    public int MaxHp => maxHp;
    
    [SerializeField] private int kill;
    public int Kill { get { return kill; } set { kill = value; } }

    [SerializeField] private int exp;

    public int Exp => exp;
    // TODO.
    [SerializeField] private int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    public int[] NextExp => nextExp;
    
    [Header("[ Game Object ]")]
    [SerializeField] private Player player;
    public Player Player => player;
    [SerializeField] private PoolManager pool;
    public PoolManager Pool => pool;
    [SerializeField] private LevelUpPop levelUpPop;
    public LevelUpPop LevelUpPop => levelUpPop;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        hp = maxHp;
        
        //TODO. Test
        levelUpPop.Select(0);
    }

    private void Update()
    {
        if (!isLive)
            return;
        
        _gameTime += Time.deltaTime;

        if (_gameTime > _maxGameTime)
            _gameTime = _maxGameTime;
    }

    public void GetExp()
    {
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
        Time.timeScale = 0f;
    }
    
    public void TimeResume()
    {
        isLive = true;
        Time.timeScale = 1f;
    }
}
