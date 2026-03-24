using UnityEngine;

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
        if (_textShown) 
        {
            _time += Time.deltaTime;
        }

        if (_time > _duration)
        {
            m_TextMeshPro.text = "";

            _textShown = false;
            _time = 0.0f;
            _duration = 0.0f;
        }
    }

    public void failedInteractionText(float duration, string Text) 
    {
        m_TextMeshPro.text = Text;
        _duration = duration;

        _time = 0.0f;
        _textShown = true;      
    }
}
