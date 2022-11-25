using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHearts : MonoBehaviour
{
    public GameObject Heart;

    [SerializeField] Sprite FullHeart; 
    [SerializeField] Sprite HalfHeart; 
    [SerializeField] Sprite EmptyHeart; 

    List<Image> Hearts = new();

    PlayerHealth playerHealth => PlayerHealth.Singleton;

    private void Awake()
    {
        for (int i = 0; i < Mathf.RoundToInt(playerHealth.MaxHealth / 2); i++)
        {
            Hearts.Add(Instantiate(Heart, transform).GetComponentInChildren<Image>());   
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < playerHealth.MaxHealth; i+=2)
        {
            int j = Mathf.RoundToInt(i / 2);
            int healthDifference = playerHealth.Health - i;

            if (healthDifference <= 0 && Hearts[j].sprite != EmptyHeart) Hearts[j].sprite = EmptyHeart;
            if (healthDifference == 1 && Hearts[j].sprite != HalfHeart) Hearts[j].sprite = HalfHeart;
            if (healthDifference >= 2 && Hearts[j].sprite != FullHeart) Hearts[j].sprite = FullHeart;
        }    
    }
}                                                                                 
