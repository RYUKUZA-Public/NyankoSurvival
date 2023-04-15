using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("[ Game Control ]")]
    [SerializeField] private float _gameTime;
    public float GameTime => _gameTime;
    [SerializeField] private float _maxGameTime = 3 * 10f;
    public float MaxGameTime => _maxGameTime;
    
    [Header("[ Player Info ]")]
    [SerializeField] private int level;
    [SerializeField] private int kill;
    public int Kill { get { return kill; } set { kill = value; } }

    [SerializeField] private int exp;
    // TODO.
    [SerializeField] private int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    
    [Header("[ Game Object ]")]
    [SerializeField] private Player player;
    public Player Player => player;
    [SerializeField] private PoolManager pool;
    public PoolManager Pool => pool;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        _gameTime += Time.deltaTime;

        if (_gameTime > _maxGameTime)
            _gameTime = _maxGameTime;
    }

    public void GetExp()
    {
        exp++;

        if (exp == nextExp[level])
        {
            level++;
            exp = 0;
        }
    }
}
