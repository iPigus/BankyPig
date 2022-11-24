using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PromptSystem : MonoBehaviour
{
    public static PromptSystem Singleton { get; private set; }

    public static int InputType = 0;

    [SerializeField] Image PromptBackground;
    [SerializeField] Image Image;
    [SerializeField] TextMeshProUGUI PromptText;

    [SerializeField] Sprite eKey;  
    [SerializeField] Sprite qKey;  
    [SerializeField] Sprite spacebarKey;  

    private void Awake()
    {
        Singleton = this;


        if(PromptBackground.color != new Color(0,0,0,0)) PromptBackground.color = new(0,0,0,0);
        if(Image.color != new Color(0,0,0,0)) Image.color = new(0,0,0,0);
        if(PromptText.color != new Color(0,0,0,0)) PromptText.color = new(0,0,0,0);
    }

    public static void SetPromptTo(int keybutton = 0, string text = "Interact")
    {
        Singleton.PromptBackground.color = new Color32(123, 152, 183, 255);
        Singleton.Image.color = Color.white;
        Singleton.PromptText.color = new Color32(35, 35, 35, 255);

        switch (keybutton)
        {
            case 0: Singleton.Image.sprite = Singleton.eKey; break;
            case 1: Singleton.Image.sprite = Singleton.qKey; break;
            case 2: Singleton.Image.sprite = Singleton.spacebarKey; break;

            default:break;
        }

        Singleton.Image.SetNativeSize();

        Singleton.PromptText.text = text;
    }

    static Coroutine FadingOutCoroutine = null;

    public static void TurnPromptOff()
    {
        if (FadingOutCoroutine != null) return;

        FadingOutCoroutine = Singleton.StartCoroutine(FadeOut(Singleton.PromptBackground));
        FadingOutCoroutine = Singleton.StartCoroutine(FadeOut(Singleton.PromptText));
        FadingOutCoroutine = Singleton.StartCoroutine(FadeOut(Singleton.Image));
    }
    static IEnumerator FadeOut(Graphic graphic, bool skipAnimation = true)
    {
        if (skipAnimation)
        {
            graphic.color = new(0, 0, 0, 0);

            yield break;
        }

        int frameRate = Mathf.RoundToInt(1 / Time.deltaTime);

        if (frameRate < 50)
        {
            if (frameRate < 30) frameRate = 30;

            for (float i = 0; i < frameRate; i++)
            {
                float color = 1 - i / frameRate;

                graphic.color = new(graphic.color.r, graphic.color.g, graphic.color.b, color);

                yield return new WaitForEndOfFrame();
            }


        }
        else
        {
            for (float i = 0; i < 50; i++)
            {
                float color = 1 - i / 50;

                graphic.color = new(graphic.color.r, graphic.color.g, graphic.color.b, color);

                yield return new WaitForFixedUpdate();
            }
        }

        graphic.color = new(0, 0, 0, 0);
        FadingOutCoroutine = null;

        yield break;
    }
}
