using System;
using UnityEngine;
using TMPro;
using System.Collections;

public class MouseTextDisplay : MonoBehaviour
{
    //public static Action<int> OnText;

    public TextMeshProUGUI tmpText;
    public Vector2 offset = new Vector2(0, -20);

    private Coroutine hideRoutine;

    void Awake()
    {
        tmpText.gameObject.SetActive(false); // Zu Beginn unsichtbar
    }

    private void OnEnable()
    {
        BonusItem.OnBonusReleased += DisplayGain;
        OnClickCreator.OnSpendPoints += DisplayCost;
        StreetObstacle.OnPointsSpend += DisplayCost;
    }

    private void OnDisable()
    {
        BonusItem.OnBonusReleased -= DisplayGain;
        OnClickCreator.OnSpendPoints -= DisplayCost;
        StreetObstacle.OnPointsSpend -= DisplayCost;
    }

    void Update()
    {
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tmpText.canvas.transform as RectTransform,
            Input.mousePosition,
            tmpText.canvas.worldCamera,
            out localMousePos
        );

        tmpText.rectTransform.anchoredPosition = localMousePos + offset;
    }

    void DisplayGain(int value)
    {
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        string valueAsString = value.ToString();

        tmpText.text = "+" + valueAsString;

        //Debug.Log(value);

        tmpText.color = Color.green;

        tmpText.gameObject.SetActive(true);
                       

        hideRoutine = StartCoroutine(HideAfterSeconds(0.25f));
    }

    void DisplayCost(int value)
    {
        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        string valueAsString = value.ToString();

        tmpText.text = "-" + valueAsString;

        //Debug.Log(value);

        tmpText.color = Color.red;

        tmpText.gameObject.SetActive(true);


        hideRoutine = StartCoroutine(HideAfterSeconds(0.25f));
    }

    IEnumerator HideAfterSeconds(float duration)
    {
        yield return new WaitForSeconds(duration);
        tmpText.gameObject.SetActive(false);
    }
}
