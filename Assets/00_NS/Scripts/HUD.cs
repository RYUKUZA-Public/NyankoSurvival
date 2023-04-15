using System;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp, Level, Kill, Time, Hp }
    [SerializeField]
    private InfoType type;

    private Text myText;
    private Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float currentExp = GameManager.Instance.Exp;
                float maxExp = GameManager.Instance.NextExp[GameManager.Instance.Level];
                mySlider.value = currentExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = $"Lv.{GameManager.Instance.Level:F0}";
                break;
            case InfoType.Kill:
                myText.text = $"{GameManager.Instance.Kill:F0}";
                break;
            case InfoType.Time:
                float remainTime = GameManager.Instance.MaxGameTime - GameManager.Instance.GameTime;
                int min = Mathf.FloorToInt(remainTime / 60); 
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = $"{min:D2}:{sec:D2}";
                break;
            case InfoType.Hp:
                float currentHp = GameManager.Instance.Hp;
                float maxHp = GameManager.Instance.MaxHp;
                mySlider.value = currentHp / maxHp;
                break;
        }
    }
}
