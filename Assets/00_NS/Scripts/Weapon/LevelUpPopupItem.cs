using UnityEngine;
using UnityEngine.UI;
using static System.String;

public class LevelUpPopupItem : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;
    public GearUpgrader gearUpgrader;


    public Image icon;
    public Text levelText;
    private Text nameText;
    private Text descText;


    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        levelText = texts[0];
        nameText = texts[1];
        descText = texts[2];

        nameText.text = data.itemName;
    }

    private void OnEnable()
    {
        levelText.text = $"Lv.{level + 1}";

        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                descText.text = Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case ItemData.ItemType.AtkSpeed:
            case ItemData.ItemType.MoveSpeed:
                descText.text = Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                descText.text = Format(data.itemDesc);
                break;
        }
    }
    
    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemData.ItemType.Melee:
            case ItemData.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeaopn = new GameObject();
                    weapon = newWeaopn.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];
                    
                    weapon.WeaponLevelUp(nextDamage, nextCount);
                }
                level++;
                break;
            case ItemData.ItemType.AtkSpeed:
            case ItemData.ItemType.MoveSpeed:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gearUpgrader = newGear.AddComponent<GearUpgrader>();
                    gearUpgrader.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gearUpgrader.GearLevelUp(nextRate);
                }
                level++;
                break;
            case ItemData.ItemType.Heal:
                GameManager.Instance.Hp = GameManager.Instance.MaxHp;
                break;
        }
        
        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
