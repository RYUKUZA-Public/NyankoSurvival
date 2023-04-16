using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // Basic

        name = $"Gear {data.itemId}";
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // property
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void GearLevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    private void ApplyGear()
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                MoveSpeedUp();
                break;
        }
    }

    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            switch (weapon.Id)
            {
                case 0:
                    weapon.Speed = 150 + (150 * rate);
                    break;
                default:
                    weapon.Speed = 0.5f * (1f - rate);
                    break;
            }
        }
    }

    private void MoveSpeedUp()
    {
        float speed = 3;
        GameManager.Instance.Player.Speed = speed + (speed * rate);
    }
}
