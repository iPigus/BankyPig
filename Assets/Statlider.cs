using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Statlider : MonoBehaviour
{
    Image Statbar; Image StatbarFill; TextMeshProUGUI Text; 

    [SerializeField] int Max = 5;
    [SerializeField] int Progress = 0;
    public bool ShowProgress = true;
    public bool ShowReadyText = true;

    float statbarWidth;
    float statbarHeight;

    private void Awake()
    {
        Statbar = GetComponent<Image>(); 
        StatbarFill = transform.GetChild(1).GetComponent<Image>();
        Text = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        statbarWidth = transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x;
        statbarHeight = StatbarFill.rectTransform.sizeDelta.y;
    }
    private void Update()
    {
        UpdateProgress();
    }
    void UpdateProgress()
    {
        Progress = Mathf.Max(Progress, 0);
        Progress = Mathf.Min(Progress, Max);

        StatbarFill.rectTransform.sizeDelta = new(statbarWidth * ((float)Progress / (float)Max), statbarHeight);
        StatbarFill.rectTransform.anchoredPosition = new(-(statbarWidth - StatbarFill.rectTransform.sizeDelta.x) / 2, 0);

        if (ShowReadyText && Progress == Max)
        {
            Text.gameObject.SetActive(true);
            Text.text = "READY";

            return;
        }

        Text.gameObject.SetActive(ShowProgress); if (ShowProgress) Text.text = Progress + " / " + Max;
    }

    public void Set(int Progress)
    {
        
    }

    public void SetMax() => Set(Max);
}
