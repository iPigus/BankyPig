using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashTextMeshPro : MonoBehaviour
{
    TextMeshProUGUI text;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(0.66f);

            text.enabled = !text.enabled;
        }
    }
}
