using UnityEngine;

/// <summary>
/// TODO. TEST
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoint;

    private int _level;
    private float _timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        _level = Mathf.FloorToInt(GameManager.Instance.GameTime / 10f);

        // TODO.
        if (_timer > (_level == 0 ? 0.5f : 0.2f))
        {
            _timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        // TODO.
        GameObject enemy = GameManager.Instance.Pool.Get(_level);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}