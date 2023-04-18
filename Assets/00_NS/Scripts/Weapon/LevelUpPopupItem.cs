using UnityEngine;
using UnityEngine.UI;
using static System.String;

public class LevelUpPopupItem : MonoBehaviour
{
    [SerializeField]
    private ItemData itemData;
    public ItemData ItemData => itemData;
    
    private Weapon _weapon;
    private GearUpgrader _gearUpgrader;

    private Image _icon;
    private Text _levelText;
    private Text _nameText;
    private Text _descText;
    
    public int ItemLevel { get; set; }

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        _icon = GetComponentsInChildren<Image>()[1];
        _icon.sprite = itemData.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        _levelText = texts[0];
        _nameText = texts[1];
        _descText = texts[2];

        _nameText.text = itemData.itemName;
    }

    /// <summary>
    /// オブジェクト表示タイミングで、データを初期化
    /// </summary>
    private void OnEnable()
    {
        _levelText.text = $"Lv.{ItemLevel + 1}";

        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                _descText.text = Format(itemData.itemDesc, itemData.damages[ItemLevel] * 100, itemData.counts[ItemLevel]);
                break;
            case ItemData.ItemType.AtkSpeed:
            case ItemData.ItemType.MoveSpeed:
                _descText.text = Format(itemData.itemDesc, itemData.damages[ItemLevel] * 100);
                break;
            default:
                _descText.text = Format(itemData.itemDesc);
                break;
        }
    }
    
    /// <summary>
    /// アイテムを選択したとき
    /// </summary>
    public void OnClick()
    {
        switch (itemData.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (ItemLevel == 0)
                {
                    // 武器アイテムを初めて購入する場合は、武器オブジェクトを作成して初期化
                    GameObject newWeaopn = new GameObject();
                    _weapon = newWeaopn.AddComponent<Weapon>();
                    _weapon.Init(itemData);
                }
                else
                {
                    // 武器アイテムをアップグレードする場合、次のレベルに合った、ダメージと使用可能回数を計算し、アップグレード
                    float nextDamage = itemData.baseDamage;
                    int nextCount = 0;

                    nextDamage += itemData.baseDamage * itemData.damages[ItemLevel];
                    nextCount += itemData.counts[ItemLevel];
                    
                    _weapon.WeaponLevelUp(nextDamage, nextCount);
                }
                ItemLevel++;
                break;
            case ItemData.ItemType.AtkSpeed:
            case ItemData.ItemType.MoveSpeed:
                if (ItemLevel == 0)
                {
                    GameObject newGear = new GameObject();
                    _gearUpgrader = newGear.AddComponent<GearUpgrader>();
                    _gearUpgrader.Init(itemData);
                }
                else
                {
                    float nextRate = itemData.damages[ItemLevel];
                    _gearUpgrader.GearLevelUp(nextRate);
                }
                ItemLevel++;
                break;
            case ItemData.ItemType.Heal:
                // 回復アイテムの場合、Playerの体力を最大体力に回復
                GameManager.Instance.Hp = GameManager.Instance.MaxHp;
                break;
        }
        
        // 最大レベル達成時、クリック禁止
        if (ItemLevel == itemData.damages.Length)
            GetComponent<Button>().interactable = false;
    }
}
