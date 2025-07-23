using UnityEngine;

public class ChangeScriptableObject : MonoBehaviour
{
    [SerializeField]
    OnClickCreator player;

    [SerializeField]
    PlayerOptionSettings providedOption;


    public void OnChangeScriptableObject()
    {
        player.ToCreateOnClick = providedOption;
    }
}
