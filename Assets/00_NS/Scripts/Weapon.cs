using System;
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

    private void Update()
    {
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                
                break;
            default:
                break;
        }
    }

    public void Init()
    {
        switch (id)
        {
            case 0:
                speed = 150;
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

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);
            
            // 近接は、無限に貫通すりので、-1 (Per)
            bullet.GetComponent<Bullet>().Init(damage, -1);
        }   
    }
}
