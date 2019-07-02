using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    [SerializeField] private Image pause_Button_Image;
    [SerializeField] private Sprite[] pause_Button_Sprites;

    [SerializeField] private Image cam_ModeSwitch_Image;

    [SerializeField] private RawImage fade_Texture;

    [SerializeField] private Color noCamSwitchColor;

    private const float MAX_COLOR_FLICKER_TIME = 2f;
    private uint currentSpriteIndex = 1;

    public static bool FadingScreen { get; private set; } = true;

    private void Awake ()
    {
        StartCoroutine(FadeInScene());      
    }

    public void ChangeTimeScale ()
    {
        if (Time.timeScale == 1.0f)
        {
            Time.timeScale = 0.0f;
            currentSpriteIndex--;
        }
        else
        {
            Time.timeScale = 1.0f;
            currentSpriteIndex++;
        }
        pause_Button_Image.sprite = pause_Button_Sprites[currentSpriteIndex];
    }

    public void ChangeButtonColor ()
    {
        CameraTranslation cam = Camera.main.GetComponent<CameraTranslation>();
        if (cam.NoSwitch || cam.InRoomConnector)
        {
            StartCoroutine(SignalNoCamSwitch());
        }       
    }

    /// <summary>
    /// Enumerator that by using mathf.pingpong and Color.lerp creates a singaling effect for the cam mode switch button
    /// </summary>
    /// <returns></returns>
    private IEnumerator SignalNoCamSwitch ()
    {     
        Color currentColor = cam_ModeSwitch_Image.color;
        float perc = 0;
        float time = 0;

        while (time < MAX_COLOR_FLICKER_TIME)
        {
            time += Time.deltaTime;
            perc = Mathf.PingPong(time, 1f);
            cam_ModeSwitch_Image.color = Color.Lerp(currentColor, noCamSwitchColor, perc);
            yield return null;
        }
    }

    private IEnumerator FadeInScene ()
    {
        FadingScreen = true;

        Color startColor = fade_Texture.color;

        while (startColor.a != 0)
        {
            startColor.a -= Time.deltaTime;
            fade_Texture.color = startColor;
            if (startColor.a < 0)
            {
                startColor.a = 0;
            }
            yield return null;
        }
        fade_Texture.gameObject.SetActive(false);
        FadingScreen = false;
    }
}
