using System;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public ItemData data;
    public int level;
    public Weapon weapon;

    public Image icon;
    public Text levelText;

    private void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        levelText = texts[0];
    }

    private void LateUpdate()
    {
        levelText.text = $"Lv.{level + 1}";
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
                
                break;
            case ItemData.ItemType.Glove:
                
                break;
            case ItemData.ItemType.Shoe:
                
                break;
            case ItemData.ItemType.Heal:
                
                break;
        }

        level++;

        if (level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
