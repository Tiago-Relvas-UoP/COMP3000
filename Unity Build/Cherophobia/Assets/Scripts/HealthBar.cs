using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float defaultScale = 2f;
    [SerializeField] public AnimationCurve curve;
    [SerializeField] public float smoothing;

    private Image overlay;
    private float value;
    private Vector3 scaleVector;
    private float _scaleMath;
    private float _velocity;
    private float _smoothedScale;

    private float time = 0.0f;

    public Color alpha;

    // If set to 1, then result will be inverted. For example, this allows the Alpha value for the HP-Related visuals to be 0 at the start of the game rather than be displayed when player is at full health
    [Header("Happiness Value")]
    [Range(0, 1)]
    public int InvertAlpha; 

    // Start is called before the first frame update
    private void Start()
    {
        overlay = GetComponent<Image>();
        alpha = GetComponent<Image>().color;
        //scaleVector = new(defaultScale, defaultScale, defaultScale);

        //overlay.transform.localScale = scaleVector;
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateOpacity();
        UpdateScale();
    }

    public void SetHealth(float health)
    {
        value = health;
    }

    private void UpdateOpacity() 
    {
        alpha.a = Mathf.Abs((value / 100) - InvertAlpha);
        overlay.color = alpha;
    }

    private void UpdateScale()
    {
        _scaleMath = defaultScale - Mathf.Lerp(0f, Mathf.Abs((value / 100) - InvertAlpha), value) - (defaultScale - 2.0f);
        _smoothedScale = Mathf.SmoothDamp(overlay.transform.localScale.x, _scaleMath, ref _velocity, smoothing);

        scaleVector = new Vector3(_smoothedScale, _smoothedScale, _smoothedScale);
        overlay.transform.localScale = scaleVector;

        /*
        _scaleMath = defaultScale - Mathf.Lerp(0f, Mathf.Abs((value / 100) - InvertAlpha), value) - (defaultScale - 2.0f);
        scaleVector = new Vector3(_scaleMath, _scaleMath, _scaleMath);
        overlay.transform.localScale = scaleVector;
        */



        // Debug
        time += Time.deltaTime;

        if (time >= 2.0f) 
        {
            Debug.Log("[INVERTALPHA: " + InvertAlpha + "]: UI Scale would be: " + _scaleMath);
            time = 0.0f;
        }     
    }
}
