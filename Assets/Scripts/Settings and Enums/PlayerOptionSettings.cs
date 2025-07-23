using UnityEngine;

[CreateAssetMenu]
public class PlayerOptionSettings : ScriptableObject
{
    [SerializeField]
    public GameObject PreviewObject;

    [SerializeField]
    public GameObject RealObject;

    [SerializeField]
    public int DefaultStartDuration;

    [SerializeField]
    public string displayName;

    [SerializeField]
    public int Cost;

    [SerializeField]
    public Sprite Icon;
}
