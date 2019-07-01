using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManagement : MonoBehaviour
{
    [SerializeField] private Image pause_Button_Image;
    [SerializeField] private Sprite[] pause_Button_Sprites;

    [SerializeField] private RawImage fade_Texture;

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
