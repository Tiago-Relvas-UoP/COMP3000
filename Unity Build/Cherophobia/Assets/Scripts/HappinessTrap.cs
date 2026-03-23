using UnityEngine;
using UnityEngine.UIElements;

public class HappinessTrap : MonoBehaviour
{
    [Header("Happiness Settings")]
    [SerializeField] public int HappinessReceived = 1;
    [SerializeField] public float DelayPerDot = 1.0f;
    [SerializeField] public float TimeBeforeActive = 5.0f;

    [Header("References")]
    [SerializeField] public HappinessController happinessController;
    [SerializeField] public Collider collider;
    [SerializeField] public GameObject textBase;
    [SerializeField] public GameObject textShadow;

    [Header("Text Settings")]
    [SerializeField] private float textDuration;
    [SerializeField] private float textCooldown;
    [SerializeField] private float baseTextXOffset;
    [SerializeField] private float baseTextYOffset;
    [SerializeField] private float shadowTextXOffset;
    [SerializeField] private float shadowTextYOffset;
    [SerializeField] private float maxScale;

    private float _timeSincePlayerInRange = 0.0f;
    private float _timeSinceLastIncrease = 0.0f;

    private float _xPosition;
    private float _yPosition;
    private float _xShadowPosition;
    private float _yShadowPosition;

    private float _randomScale;

    private float _textTime;
    private float _timeSinceTextShown;

    private bool _isFirstTextShown;
    private bool _startTextTimer;
    private bool _startTextCooldown;

    private bool _textInCooldow;
    private bool _firstText;
    private bool _secondText;

    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        _textTime = 0.0f;
        _timeSinceTextShown = 0.0f;

        _startTextTimer = false;
        _startTextCooldown = true;
        _isFirstTextShown = false;
        _textInCooldow = false;

        _firstText = true;
        _secondText = true;
    }

    private void Update()
    {
        if (_startTextTimer) _textTime += Time.deltaTime;
        if (_startTextCooldown) _timeSinceTextShown += Time.deltaTime;

        if (_textTime >= textDuration)
        {
            textBase.SetActive(false);
            textShadow.SetActive(false);

            _textInCooldow = true;
            _startTextCooldown = true;
            _startTextTimer = false;

            _timeSinceTextShown = 0.0f;
            _textTime = 0f;

            if (!_isFirstTextShown)
            {
                _isFirstTextShown = true;
            }
        }

        if (_timeSinceTextShown >= textCooldown) 
        {
            _startTextCooldown = false;
            _textInCooldow = false;
        }

        // Debug.Log("Time values: \n- " + _textTime + " / " + _timeSinceTextShown);
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            _timeSincePlayerInRange += Time.deltaTime;

            if (_timeSincePlayerInRange >= TimeBeforeActive)
            {
                _timeSinceLastIncrease += Time.deltaTime;

                if (_timeSinceLastIncrease >= DelayPerDot)
                {
                    _timeSinceLastIncrease = 0.0f;
                    happinessController.IncreaseHappiness(HappinessReceived);
                }

            }

            if (_timeSincePlayerInRange >= TimeBeforeActive + 3f)
            {
                if (_firstText)
                {
                    _startTextTimer = true;
                    _firstText = false;
                }

                if (!_isFirstTextShown) ShowText(textBase, textShadow, baseTextXOffset, baseTextYOffset, shadowTextXOffset, shadowTextYOffset, maxScale);
                else 
                {
                    if (_secondText) 
                    {
                        _startTextTimer = false;
                        _startTextCooldown = true;
                        _secondText = false;
                    }

                    if (!_textInCooldow)
                    {
                        _startTextTimer = true;
                        ShowText(textBase, textShadow, baseTextXOffset, baseTextYOffset, shadowTextXOffset, shadowTextYOffset, maxScale);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Debug.Log("Player has left trap zone!");
            _timeSincePlayerInRange = 0.0f;
            _timeSinceTextShown = 0.0f;
            // _startTextTimer = false;
            textBase.SetActive(false);
            textShadow.SetActive(false);

            _startTextTimer = false;
            _startTextCooldown = false;
            _isFirstTextShown = false;
            _textInCooldow = true;

            _firstText = true;
            _secondText = true;
        }
    }

    private void ShowText(GameObject baseText, GameObject shadowText, float xVariance = 500f, float yVariance = 500f, float xShadowOffset = 60f, float yShadowOffset = 60f, float scale = 0.8f)
    {
        baseText.SetActive(true);
        shadowText.SetActive(true);

        _xPosition = Random.Range(-xVariance, xVariance);
        _yPosition = Random.Range(-yVariance, yVariance);

        _xShadowPosition = Random.Range(-xShadowOffset, xShadowOffset);
        _yShadowPosition = Random.Range(-yShadowOffset, yShadowOffset);

        _randomScale = Random.Range(0.3f, scale);

        // Base Text
        baseText.transform.localScale = new Vector3(_randomScale, _randomScale, _randomScale);
        baseText.transform.localPosition = new Vector3(_xPosition, _yPosition, 0.0f);

        // Shadow Text
        shadowText.transform.localScale = new Vector3(_randomScale, _randomScale, _randomScale);
        shadowText.transform.localPosition = new Vector3
        (
            baseText.transform.localPosition.x + _xShadowPosition, 
            baseText.transform.localPosition.y + _yShadowPosition, 0.0f
        );    
    }
}
