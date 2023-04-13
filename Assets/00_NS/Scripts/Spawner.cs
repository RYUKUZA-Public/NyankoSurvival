using UnityEngine;

/// <summary>
/// TODO. TEST
/// </summary>
public class Spawner : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameManager.Instance.Pool.Get(0);
        }
    }
}
