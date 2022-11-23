using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptSystem : MonoBehaviour
{
    public static PromptSystem Singleton { get; private set; }

    [SerializeField] Image PromptBackground;
    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI PromptText;

    [SerializeField] Sprite eKey;  
    [SerializeField] Sprite qKey;  
    [SerializeField] Sprite spacebarKey;  

    private void Awake()
    {
        Singleton = this;

        if(Image == null) Image = GetComponentInChildren<Image>();
        if(PromptText == null) PromptText = GetComponentInChildren<TextMeshProUGUI>();

        SetPromptTo("Essa", 1);
    }

    public static void SetPromptTo(string text, int keybutton)
    {
        Singleton.PromptBackground.color = new(1, 1, 1, 1);
        Singleton.Image.color = new(1, 1, 1, 1);
        Singleton.PromptText.color = new(1, 1, 1, 1);

        switch (keybutton)
        {
            case 0: Singleton.Image.sprite = Singleton.eKey; break;
            case 1: Singleton.Image.sprite = Singleton.qKey; break;
            case 2: Singleton.Image.sprite = Singleton.spacebarKey; break;

            default:break;
        }

        Singleton.Image.SetNativeSize();

    }

    static Coroutine FadingOutCoroutine = null;

    public static void TurnPromptOff()
    {
        if (FadingOutCoroutine != null) return;

        FadingOutCoroutine = Singleton.StartCoroutine(FadeOut());
    }
    static IEnumerator FadeOut()
    {
        int frameRate = Mathf.RoundToInt(1 / Time.deltaTime);

        if (frameRate < 50)
        {
            if (frameRate < 30) frameRate = 30;

            for (float i = 0; i < frameRate; i++)
            {
                float color = 1 - i / frameRate;

                Singleton.PromptBackground.color = new(0, 0, 0, color);
                Singleton.Image.color = new(0, 0, 0, color);
                Singleton.PromptText.color = new(0, 0, 0, color);

                yield return new WaitForEndOfFrame();
            }


        }
        else
        {
            for (float i = 0; i < 50; i++)
            {
                float color = 1 - i / 50;

                Singleton.PromptBackground.color = new(0, 0, 0, color);
                Singleton.Image.color = new(0, 0, 0, color);
                Singleton.PromptText.color = new(0, 0, 0, color);

                yield return new WaitForFixedUpdate();
            }
        }

        Singleton.PromptBackground.color = new(0, 0, 0, 0);
        Singleton.Image.color = new(0, 0, 0, 0);
        Singleton.PromptText.color = new(0, 0, 0, 0);
        FadingOutCoroutine = null;

        yield break;
    }
}
