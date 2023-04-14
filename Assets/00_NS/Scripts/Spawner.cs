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
public class Spawner : MonoBehaviour
{

    [SerializeField]
    private SpawnData[] spawnData;
    
    private Transform[] _spawnPoint;

    private int _level;
    private float _timer;

    private void Awake()
    {
        _spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _level = Mathf.Min(Mathf.FloorToInt(GameManager.Instance.GameTime / 10f), spawnData.Length - 1 );
        
        Debug.Log(_level);

        // TODO.
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
        enemy.GetComponent<Enemy>().Init(spawnData[_level]);
    }
}