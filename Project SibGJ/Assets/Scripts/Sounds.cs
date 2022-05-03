using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public static Sounds PlaySound;
    public AudioSource fall;
    public AudioSource fire;
    public AudioSource jump;
    public AudioSource push;
    public AudioSource step;

    private void Awake()
    {
        PlaySound = this;
    }
}
