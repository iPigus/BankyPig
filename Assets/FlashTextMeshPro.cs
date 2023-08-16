using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashTextMeshPro : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] float flashTime = .66f;
    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        StartCoroutine(Flashing());
    }

    IEnumerator Flashing()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(flashTime);

            text.enabled = !text.enabled;
        }
    }
}
