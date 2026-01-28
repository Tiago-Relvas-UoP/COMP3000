using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private Image overlay;
    private float value;

    public Color alpha;

    // If set to 1, then result will be inverted. For example, this allows the Alpha value for the HP-Related visuals to be 0 at the start of the game rather than be displayed when player is at full health
    [Header("Happiness Value")]
    [Range(0, 1)]
    public int InvertAlpha; 

    // Start is called before the first frame update
    void Start()
    {
        overlay = GetComponent<Image>();
        alpha = GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        alpha.a = Mathf.Abs((value / 100) - InvertAlpha);
        overlay.color = alpha;
    }

    public void SetHealth(float health)
    {
        value = health;
    }
}
