using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private Text resultText;

    public void Lose() => resultText.text = "敗北...";
    public void Win() => resultText.text = "勝利！";
}
