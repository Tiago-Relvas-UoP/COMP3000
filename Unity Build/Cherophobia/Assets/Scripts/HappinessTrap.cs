using UnityEngine;
using UnityEngine.UIElements;

// Responsible for handling Proximity Trap, or "Happy memory/object" trap.
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

    [Header("Clue Text Settings")]
    [SerializeField] private string clueText;
    [SerializeField] private float clueDuration;
    private bool _wasClueShown;
    private InteractionFailed _interactionFailed;

    private GameManager _gameManager;

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

        _wasClueShown = false; 
        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (_startTextTimer) _textTime += Time.deltaTime; // If flag is enable, start text timer countdown
        if (_startTextCooldown) _timeSinceTextShown += Time.deltaTime; // If flag is enable, start text timer cooldown countdown.

        // Disable text game objects if text duration has passed the countdown, and reset the relevant flags.
        if (_textTime >= textDuration)
        {
            textBase.SetActive(false);
            textShadow.SetActive(false);

            _textInCooldow = true;
            _startTextCooldown = true;
            _startTextTimer = false;

            _timeSinceTextShown = 0.0f;
            _textTime = 0f;

            // If its first shown text, then enable its flag to true to determine it was already shown once.
            if (!_isFirstTextShown)
            {
                _isFirstTextShown = true;
            }
        }

        // If enough time has passed since text was shown, then disable relevant cooldown flags.
        if (_timeSinceTextShown >= textCooldown) 
        {
            _startTextCooldown = false;
            _textInCooldow = false;
        }
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider collider)
    {
        // If Player enters in trigger range
        if (collider.gameObject.tag == "Player")
        {
            _gameManager.insideTrap = true;
            _timeSincePlayerInRange += Time.deltaTime; // Start contdown since they entered range

            // If enough time has passed (Roughly half the time before trap becomes active, and clue was not shown yet), then show clue to player regarding dangerous/happy object nearby.
            if (_timeSincePlayerInRange >= Mathf.Floor(TimeBeforeActive / 1.5f) && !_wasClueShown) 
            {
                _interactionFailed.failedInteractionText(clueDuration, clueText);
                _wasClueShown = true;
            } 

            // If enough time has passed, enable trap
            if (_timeSincePlayerInRange >= TimeBeforeActive)
            {
                _timeSinceLastIncrease += Time.deltaTime; // Track time since last increase in happiness by the trap

                // If tracked time is higher than delay timer, then increase happiness and reset timer to default. Happiness is added in small dots rather than continously
                if (_timeSinceLastIncrease >= DelayPerDot)
                {
                    _timeSinceLastIncrease = 0.0f;
                    happinessController.IncreaseHappiness(HappinessReceived);
                    PlayerPrefs.SetFloat("lastDeathCause", 1f);
                }

            }

            // If player has been in range of an active trap for sometime, display a specialized text.
            if (_timeSincePlayerInRange >= TimeBeforeActive + 3f)
            {
                // if flag that tracks first text is enabled, disable the flag and start timer to countdown text duration
                if (_firstText)
                {
                    _startTextTimer = true;
                    _firstText = false;
                }

                if (!_isFirstTextShown) ShowText(textBase, textShadow, baseTextXOffset, baseTextYOffset, shadowTextXOffset, shadowTextYOffset, maxScale); // Show first text if not shown already
                else // If first text already shown
                {
                    if (_secondText)  // If second text, reset its flag, disable text timer and start cooldown
                    {
                        _startTextTimer = false;
                        _startTextCooldown = true;
                        _secondText = false;
                    }

                    // If text is not in cooldown, then start text timer and call upon ShowText() to display text on screen
                    if (!_textInCooldow)
                    {
                        _startTextTimer = true;
                        ShowText(textBase, textShadow, baseTextXOffset, baseTextYOffset, shadowTextXOffset, shadowTextYOffset, maxScale);
                    }
                }
            }
        }
    }

    // Reset necessary flags/objects once player leaves trap range
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
            _gameManager.insideTrap = false;

            _firstText = true;
            _secondText = true;
        }
    }

    // Method responsible for showing Text on screen once trap is active. Handles two texts, a base text and a shadow (outline) text.
    // Takes into account position offset and scale variance for both texts, which are random based on given offset/scale range.
    // Shadow position is based on base text position, with an additional random offset added on top of the base text position so they are never far apart.
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
