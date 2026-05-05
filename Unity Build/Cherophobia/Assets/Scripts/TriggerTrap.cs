using UnityEngine;
using UnityEngine.UI;

// Handles the Trigger Trap (or Happiness Spray Trap) behaviour.
public class TriggerTrap : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] public int HappinessReceived = 25;
    [SerializeField] public bool isTrapActive;
    [SerializeField] public GameObject trapMesh;

    [Header("References")]
    [SerializeField] public HappinessController happinessController;
    [SerializeField] public Collider collider;
    [SerializeField] public GameObject NormalText;
    [SerializeField] public GameObject ShadowText;

    [Header("Audio On Trigger")]
    [SerializeField] public AudioClip triggerAudio;
    [SerializeField, Range(0f, 1f)] public float triggerVolume;
    [SerializeField] public AudioClip jumpscareAudio;
    [SerializeField, Range(0f, 1f)] public float jumpscareVolume;

    [Header("Trigger Sprites")]
    [SerializeField] private GameObject imageHolder;
    [SerializeField] private Sprite[] images;
    // [SerializeField] private GameObject[] images;
    [SerializeField] private float imageDuration;
    [SerializeField] private int imageQuantity;
    [SerializeField] private float imageScale;
    [SerializeField] private float xPositionVariant;
    [SerializeField] private float yPositionVariant;

    [Header("Text Settings")]
    [SerializeField] public float durationAfterJumpscare = 2f;
    [SerializeField] public float basePositionOffset = 20f;
    [SerializeField] public float baseScaleOffset = 1f;
    [SerializeField] public float shadowPositionOffset = 20f;
    [SerializeField] public float shadowScaleOffset = 1f;

    [Header("Clue Text Settings")]
    [SerializeField] private string clueText;
    [SerializeField] private float clueDuration;
    private bool _wasClueShown;
    private InteractionFailed _interactionFailed;


    private bool _triggered;
    private bool _jumpscareComplete;
    private float _xPosition;
    private float _yPosition;
    private float _time;
    private float _textTime;
    private int _imageIndex;
    private int _imageCount;
    private bool _firstImageShowed;
    private bool _trapComplete;

    // Start is called before the first frame update
    void Start()
    {
        _time = 0f;
        _textTime = 0f;

        collider = GetComponent<Collider>();
        _imageCount = 0;
        _jumpscareComplete = false;
        _firstImageShowed = false;
        _trapComplete = false;

        _wasClueShown = false;
        _interactionFailed = GameObject.FindGameObjectWithTag("FailedText").GetComponent<InteractionFailed>();
    }

    private void Update()
    {
        ShowMesh();
        trapMesh.SetActive(ShowMesh());
        if (_triggered && !_jumpscareComplete) ImageJumpscare(); // Play Jumspcare sequence once trap is triggered
        if (_triggered) ShowText(); // Show Text after jumpscare, once trap is triggered

    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider collider)
    {
        // Show clue text once trap is active for the first time.
        if (isTrapActive && !_wasClueShown) 
        {
            _interactionFailed.failedInteractionText(clueDuration, clueText);
            _wasClueShown = true;
        }

        // Play appropriate SFX once trap is active, increase Happiness and change appropriate flags to disable trap and indicates its been triggered.
        if (isTrapActive)
        {
            AudioManager.instance.PlaySFX(triggerAudio, triggerVolume);
            AudioManager.instance.PlaySFX(jumpscareAudio, jumpscareVolume);

            PlayerPrefs.SetFloat("lastDeathCause", 2f);

            happinessController.IncreaseHappiness(25);
            isTrapActive = false;
            _triggered = true;
        }
    }

    // Changes Mesh depending if trap was triggered + completed.
    private bool ShowMesh()
    {
        if (!_trapComplete) return true;
        else return false;
    }

    // Plays sequence of images (jumpscare)
    private void ImageJumpscare() 
    {
        // If not shown already, show first image and enable flag that tracks its been shown.
        if (!_firstImageShowed) 
        {
            imageHolder.SetActive(true);

            _imageIndex = Random.Range(0, images.Length);
            ShowImage(_imageIndex, xPositionVariant, yPositionVariant, imageScale);

            _firstImageShowed = true;
        }

        // If the amount of images shown is below the set quantity value, keep showing images.
        if (_imageCount <= imageQuantity)
        {
            _time += Time.deltaTime;
            
            // If enough time has passed, show new image and reset timer.
            if (_time > imageDuration)
            {
                _imageIndex = Random.Range(0, images.Length);
                ShowImage(_imageIndex, xPositionVariant, yPositionVariant, imageScale);

                _time = 0.0f;
                _imageCount++;
            }
        }

        // Disable Images game object and mark object as completed.
        if (_imageCount > imageQuantity)
        {
            imageHolder.SetActive(false);

            _jumpscareComplete = true;
        }
    }

    // Shows Text during and after jumpscare is completed.
    private void ShowText() 
    { 
        // If not completed, show appropriate text.
        if (!_jumpscareComplete) 
        {
            ShowImage(0, basePositionOffset, basePositionOffset, baseScaleOffset, NormalText);
            ShowImage(0, shadowPositionOffset, shadowPositionOffset, baseScaleOffset, ShadowText);
        }

        // If completed, show appropriate text.
        if (_jumpscareComplete) 
        {
            _textTime += Time.deltaTime; // Countdown duration text is on screen.
            //Debug.Log("Current Timer [Jumpscare complete]: " + _textTime);

            // Change text position each frame.
            ShowImage(0, basePositionOffset, basePositionOffset, baseScaleOffset, NormalText);
            ShowImage(0, shadowPositionOffset, shadowPositionOffset, baseScaleOffset, ShadowText);

            // If enough time has passed, disable all text objects, and mark trap as complete.
            if (_textTime > durationAfterJumpscare) 
            {
                NormalText.SetActive(false);
                ShadowText.SetActive(false);

                _triggered = false;
                _trapComplete = true;

                // Show clue regarding how to avoid trigger traps if not shown already, after trap is completed.
                if (!_wasClueShown)
                {
                    _interactionFailed.failedInteractionText(clueDuration, clueText);
                    _wasClueShown = true;
                }
            }
        }
    }

    // If no game object is provided, changes scale and positon of the image game object each frame based on given offset input.
    // Else, if game object is provided, change from that game object instead (for text) and set as true.
    private void ShowImage(int index, float xVariance = 0, float yVariance = 0, float scale = 1, GameObject gameObject = null) 
    {
        _xPosition = Random.Range(-xVariance, xVariance);
        _yPosition = Random.Range(-yVariance, yVariance);
        //Debug.Log("X Position: " + _xPosition);
        //Debug.Log("X Position: " + _yPosition);

        if (gameObject == null) 
        {
            imageHolder.GetComponent<Image>().sprite = images[_imageIndex];
            imageHolder.transform.localScale = new Vector3(scale, scale, scale);
            imageHolder.transform.localPosition = new Vector3(_xPosition, _yPosition, 0.0f);

        }
        else 
        {
            gameObject.transform.localScale = new Vector3(scale, scale, scale);
            gameObject.transform.localPosition = new Vector3(_xPosition, _yPosition, 0.0f);

            gameObject.SetActive(true);
        }
    }
}
