using UnityEngine;

[System.Serializable]
public class SpawnData
{
    public float SpawnTime;
    public int Hp;
    public float Speed;
}

/// <summary>
/// TODO. TEST
/// </summary>
public class SpawnController : NewMonoBehaviour
{

    [SerializeField]
    private SpawnData[] spawnData;

    private float _levelTime;
    
    private Transform[] _spawnPoint;

    private int _level;
    private float _timer;

    private void Awake()
    {
        _spawnPoint = GetComponentsInChildren<Transform>();
        _levelTime = GameManager.Instance.MaxGameTime / spawnData.Length;
    }

    public override void NewUpdate()
    {
        if (!GameManager.Instance.IsLive)
            return;
        
        _timer += Time.deltaTime;
        _level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / _levelTime), spawnData.Length - 1 );

        if (_timer > spawnData[_level].SpawnTime)
        {
            _timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        // TODO.
        GameObject enemy = GameManager.Instance.Pool.Get(PoolType.Monster, _level);
        enemy.transform.position = _spawnPoint[Random.Range(1, _spawnPoint.Length)].position;
        enemy.GetComponent<EnemyController>().Init(spawnData[_level]);
    }
}