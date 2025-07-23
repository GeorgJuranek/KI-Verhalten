using UnityEngine;
using TMPro;

public class PointsDisplayer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI valueText;

    [SerializeField] PointsKeeper pointsKeeper;

    private void Awake()
    {
        pointsKeeper.OnPointsChanged += UpdateText;

        UpdateText(pointsKeeper.Points);
    }

    private void UpdateText(int newValue)
    {
        valueText.text = newValue.ToString();
    }

    private void OnDestroy()
    {
        pointsKeeper.OnPointsChanged -= UpdateText;
    }
}
