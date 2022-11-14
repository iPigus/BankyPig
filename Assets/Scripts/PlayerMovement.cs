using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Controls Controls { get; private set; }

    private void Awake()
    {
        Controls = new();


    }




    #region Inputs stuff
    private void OnEnable() => Controls.Enable();
    private void OnDisable() => Controls.Disable();
    #endregion
}
