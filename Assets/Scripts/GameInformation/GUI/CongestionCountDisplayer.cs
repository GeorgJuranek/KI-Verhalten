using TMPro;
using UnityEngine;

public class CongestionCountDisplayer : MonoBehaviour
{
    int currentCount;
    int previousCount;

    [SerializeField]
    TextMeshProUGUI congestionCount;


    private void OnEnable()
    {
        CongestedCarsObserver.OnCountChanged += UpdateCongestionDisplay;
    }

    private void OnDisable()
    {
        CongestedCarsObserver.OnCountChanged -= UpdateCongestionDisplay;
    }

    void UpdateCongestionDisplay(int newCount)
    {
        previousCount = currentCount;
        currentCount = newCount;

        int result = (int)(Mathf.Lerp(previousCount, currentCount, Time.deltaTime));

        UpdateText(result.ToString());
    }

    void UpdateText(string message)
    {
        congestionCount.text = message.ToString();
    }
}
