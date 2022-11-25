using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadSystem : MonoBehaviour
{
    public static DeadSystem Singleton { get; private set; }

    [SerializeField] GameObject DeadUI;

    public static bool isDeadUIactive => Singleton.DeadUI.activeSelf;

    private void Awake()
    {
        Singleton = this;
    }

    public static void TurnOn()
    {
        PromptSystem.TurnOffAllPrompts();
        Singleton.DeadUI.SetActive(true);
    }
    public static void TurnOff()
    {
        Singleton.DeadUI.SetActive(false);

    }
}
