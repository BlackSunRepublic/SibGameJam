using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallSounder : MonoBehaviour
{
    public void PlayFall()
    {
        Sounds.PlaySound.fall.Play();
    }
}
