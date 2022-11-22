using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInSystem : MonoBehaviour
{
    public static FadeInSystem Singleton { get; private set; }

    [SerializeField] Image FadeTexture;

    private void Awake()
    {
        Singleton = this;

        FadeOut();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            FadeIn();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            FadeOut();
        }
    }

    public static void FadeIn()
    {
        Singleton.StopAllCoroutines();

        Singleton.StartCoroutine(Singleton.FadeI());
    }

    public static void FadeOut()
    {
        Singleton.StopAllCoroutines();

        Singleton.StartCoroutine(Singleton.FadeO());
    }

    IEnumerator FadeI()       
    {
        int frameRate = Mathf.RoundToInt(1 / Time.deltaTime);

        if (frameRate < 50)
        {
            if(frameRate < 30) frameRate = 30;

            for (float i = 0; i < frameRate; i++)
            {
                float color = i / frameRate;

                FadeTexture.color = new(0, 0, 0, color);

                yield return new WaitForEndOfFrame();
            }
            

        }
        else
        {
            for (float i = 0; i < 50; i++)
            {
                float color = i / 50;

                FadeTexture.color = new(0, 0, 0, color);

                yield return new WaitForFixedUpdate();
            }
        }

        FadeTexture.color = new(0,0,0, 1);

        yield break;
    }
    IEnumerator FadeO()
    {
        int frameRate = Mathf.RoundToInt(1 / Time.deltaTime);

        if (frameRate < 50)
        {
            if (frameRate < 30) frameRate = 30;

            for (float i = 0; i < frameRate; i++)
            {
                float color = 1 - i / frameRate;

                FadeTexture.color = new(0,0,0, color);

                yield return new WaitForEndOfFrame();
            }


        }
        else
        {
            for (float i = 0; i < 50; i++)
            {
                float color = 1 - i / 50;

                FadeTexture.color = new(0,0,0, color);

                yield return new WaitForFixedUpdate();
            }
        }

        FadeTexture.color = new(0,0,0, 0);

        yield break;
    }
}
