using UnityEngine;

public class GearUpgrader : MonoBehaviour
{
    /// <summary>
    /// アイテムタイプ
    /// </summary>
    private ItemData.ItemType _itemType;
    /// <summary>
    /// 強化比率
    /// </summary>
    private float _rate;

    /// <summary>
    /// データ初期化
    /// </summary>
    public void Init(ItemData data)
    {
        // Basic
        name = $"Gear {data.itemId}";
        transform.parent = GameManager.Instance.PlayerController.transform;
        transform.localPosition = Vector3.zero;

        // property
        _itemType = data.itemType;
        _rate = data.damages[0];
        ApplyGear();
    }

    /// <summary>
    /// ギアレベルアップ
    /// </summary>
    public void GearLevelUp(float rate)
    {
        this._rate = rate;
        ApplyGear();
    }

    /// <summary>
    /// 能力値強化適用
    /// </summary>
    private void ApplyGear()
    {
        switch (_itemType)
        {
            case ItemData.ItemType.AtkSpeed:
                RateUp(); // 攻撃速度の強化
                break;
            case ItemData.ItemType.MoveSpeed:
                MoveSpeedUp(); // 移動速度の強化
                break;
        }
    }

    /// <summary>
    /// 攻撃速度の強化
    /// </summary>
    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            // 0 近接, 1 遠距離
            switch (weapon.Id)
            {
                case 0:
                    // 攻撃速度の基本値 + キャラクター別固有能力
                    float speed = GameManager.Instance.MeleeWeaponBaseSpeed * CharacterSpecialAbility.WqaponSpeed;
                    weapon.Speed = speed + (GameManager.Instance.MeleeWeaponBaseSpeed * _rate);
                    break;
                default:
                    // 攻撃速度の基本値 + キャラクター別固有能力
                    speed = GameManager.Instance.RangeWeaponBaseSpeed * CharacterSpecialAbility.WqaponRate;
                    weapon.Speed = speed * (1f - _rate);
                    break;
            }
        }
    }

    private void MoveSpeedUp()
    {
        // 移動速度の基本値 + キャラクター別固有能力
        float speed = GameManager.Instance.PlayerBaseMoveSpeed * CharacterSpecialAbility.Speed;
        GameManager.Instance.PlayerController.Speed = speed + (speed * _rate);
    }
}
