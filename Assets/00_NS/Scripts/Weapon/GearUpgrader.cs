using UnityEngine;

public class GearUpgrader : MonoBehaviour
{
    public ItemData.ItemType type;
    public float rate;

    public void Init(ItemData data)
    {
        // Basic

        name = $"Gear {data.itemId}";
        transform.parent = GameManager.Instance.PlayerController.transform;
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
            case ItemData.ItemType.AtkSpeed:
                RateUp();
                break;
            case ItemData.ItemType.MoveSpeed:
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
                    float speed = 150 * CharacterSpecialAbility.WqaponSpeed;
                    weapon.Speed = speed + (150 * rate);
                    break;
                default:
                    speed = 0.5f * CharacterSpecialAbility.WqaponRate;
                    weapon.Speed = speed * (1f - rate);
                    break;
            }
        }
    }

    private void MoveSpeedUp()
    {
        float speed = 3 * CharacterSpecialAbility.Speed;
        GameManager.Instance.PlayerController.Speed = speed + (speed * rate);
    }
}
