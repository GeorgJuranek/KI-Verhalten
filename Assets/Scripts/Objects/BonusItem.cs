using UnityEngine;
using System;
using TMPro;

public class BonusItem : MonoBehaviour, IClickable
{
    public static event Action<int> OnBonusReleased;

    [SerializeField]
    int amount = 5;

    [SerializeField]
    int cost = 0;
    public int Cost { get => cost; }

    SpriteRenderer spriteRenderer;

    TextMeshPro text;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        text = GetComponentInChildren<TextMeshPro>();
    }

    public void LeftClick()
    {
        OnBonusReleased(amount);
        Destroy(this.gameObject);
    }

    public void RightClick()
    {
        OnBonusReleased(amount);
        Destroy(this.gameObject);
    }

    public void Hover(Color newColor)
    {
        spriteRenderer.color = newColor;
        text.color = newColor;
    }

    public void Unhover()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.color = Color.white;
        text.color = Color.white;
    }
}
