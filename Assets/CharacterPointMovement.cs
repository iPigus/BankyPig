using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPointMovement : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
