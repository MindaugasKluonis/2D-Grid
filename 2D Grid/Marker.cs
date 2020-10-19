using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer rend;

    public void SetSprite(Sprite sprite)
    {
        rend.sprite = sprite;
    }

    public void SetOpacity(float opacity)
    {
        Color color = rend.color;
        color.a = opacity;
        rend.color = color;
    }

    public void SetSpriteAndOpacity(Sprite sprite, float opacity)
    {
        SetSprite(sprite);
        SetOpacity(opacity);
    }

    public void SetColor(Color color)
    {
        rend.color = color;
    }
}
