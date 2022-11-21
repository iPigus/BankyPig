using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHearts : MonoBehaviour
{
    public GameObject Heart;

    PlayerHealth playerHealth => PlayerHealth.Singleton;

    private void Awake()
    {
        for (int i = 0; i < Mathf.RoundToInt(playerHealth.MaxHealth / 2); i++)
        {
            Instantiate(Heart, transform);   
        }
    }
}                                                                                 
