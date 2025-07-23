using System.Collections;
using UnityEngine;
using TMPro;

public class ErrorDisplayer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI errorText;

    [SerializeField]
    float duration;

    private void OnEnable()
    {
        OnClickCreator.OnError += ShowErrorMessage;
    }

    private void OnDisable()
    {
        OnClickCreator.OnError -= ShowErrorMessage;
    }

    void ShowErrorMessage(string errorMessage)
    {
        StartCoroutine(DisplayMessageForWhile(errorMessage));
    }

    IEnumerator DisplayMessageForWhile(string newMessage)
    {
        errorText.text = newMessage;

        float timer = duration;

        while (timer>0)
        {
            yield return new WaitForSeconds(Time.deltaTime);

            timer -= Time.deltaTime;
        }

        errorText.text = "";
    }
}
