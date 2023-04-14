using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [SerializeField] private Player player;
    public Player Player => player;
    
    [SerializeField] private PoolManager pool;
    public PoolManager Pool => pool;

    private float _gameTime;
    public float GameTime => _gameTime;
    private float _maxGameTime = 3 * 10f;
    public float MaxGameTime => _maxGameTime;

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
}
