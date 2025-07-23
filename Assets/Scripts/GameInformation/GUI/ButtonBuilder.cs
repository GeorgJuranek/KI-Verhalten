using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ButtonBuilder : MonoBehaviour
{
    [SerializeField]
    PlayerOptionSettings providedOption;

    [SerializeField]
    TextMeshProUGUI targetLabelText;

    [SerializeField]
    Image targetDisplayGraphic;

    [SerializeField]
    TextMeshProUGUI costLabelText;



    void Awake()
    {
        targetLabelText.text = providedOption.displayName;

        Sprite newSprite = providedOption.Icon;

        targetDisplayGraphic.sprite = newSprite;

        costLabelText.text = providedOption.Cost.ToString();
    }
}
