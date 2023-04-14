using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int prefabId;
    [SerializeField]
    private float damage;
    [SerializeField]
    private float count;
    [SerializeField]
    private float speed;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                // 回転方向のせいで -
                speed = -150;
                Place();
                break;
            default:
                break;
        }
    }

    private void Place()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet = GameManager.Instance.Pool.Get(PoolType.Weapon, prefabId).transform;
            bullet.transform.parent = transform;
            // 近接は、無限に貫通すりので、-1 (Per)
            bullet.GetComponent<Bullet>().Init(damage, -1);
        }   
    }
}
