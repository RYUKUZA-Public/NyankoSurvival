using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float damage;
    public float Damage => damage;
    [SerializeField]
    private int per;

    public int Per => per;

    public void Init(float damage, int per)
    {
        this.damage = damage;
        this.per = per;
    }
}
