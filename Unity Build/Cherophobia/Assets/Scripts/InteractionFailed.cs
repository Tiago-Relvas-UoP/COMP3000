using TMPro;
using UnityEngine;

// Called externally to show an Interact Text when failing a certain interaction, but also for other uses such as hints.
public class InteractionFailed : MonoBehaviour
{
    public TMPro.TextMeshProUGUI m_TextMeshPro;

    private float _time;
    private float _duration;
    private bool _textShown;

    private void Start()
    {
        m_TextMeshPro = this.GetComponent<TMPro.TextMeshProUGUI>();
        m_TextMeshPro.text = "";

        _time = 0.0f;
        _duration = 0.0f;
        _textShown = false;
    }

    private void Update()
    {
        // Only start time countdown if text is on screen, determined by a flag
        if (_textShown) 
        {
            _time += Time.deltaTime;
        }

        // Once enough time has elapsed, reset flags and text back to default state.
        if (_time > _duration)
        {
            m_TextMeshPro.text = "";

            _textShown = false;
            _time = 0.0f;
            _duration = 0.0f;
        }
    }

    // Method called externally to invoke text. Takes in multiple parameters such as text duration, and font.
    public void failedInteractionText(float duration, string Text, TMP_FontAsset font = null) 
    {
        m_TextMeshPro.text = Text;
        _duration = duration;

        _time = 0.0f;
        _textShown = true;

        if (font != null) m_TextMeshPro.font = font;
    }
}
