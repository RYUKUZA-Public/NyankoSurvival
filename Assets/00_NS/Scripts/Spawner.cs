using UnityEngine;

/// <summary>
/// TODO. TEST
/// </summary>
public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoint;

    private float _timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer > 0.2f)
        {
            _timer = 0;
            Spawn();
        }
    }

    private void Spawn()
    {
        // TODO.
        GameObject enemy = GameManager.Instance.Pool.Get(Random.Range(0, 4));
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
    }
}