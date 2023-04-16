using UnityEngine;

public class CharacterSpecialAbility : MonoBehaviour
{
    public static float Speed => GameManager.Instance.PlayerId == 1 ? 1.1f : 1f;
    public static float WqaponSpeed => GameManager.Instance.PlayerId == 0 ? 1.1f : 1f;
    public static float WqaponRate => GameManager.Instance.PlayerId == 0 ? 0.9f : 1f;
}
