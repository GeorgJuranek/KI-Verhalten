using UnityEngine;
using TMPro;

public class CountDownTextChanger : MonoBehaviour
{
    [SerializeField]
    TimeSelfDestructor destructor;

    [SerializeField]
    TextMeshPro count;

    private void FixedUpdate()
    {
        UpdateText(destructor.TimeTillDestroy.ToString());
    }

    void UpdateText(string newText)
    { 
        count.text = newText;
    }

}
