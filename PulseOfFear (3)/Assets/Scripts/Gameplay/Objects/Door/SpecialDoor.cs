using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class SpecialDoor : MonoBehaviour
{
    public PressurePlate plaque1;    // Référence à la première plaque de pression
    public PressurePlate plaque2;    // Référence à la deuxième plaque de pression
    private FMOD.Studio.EventInstance Door;//


    private void Start()
    {

    }

    private void Update()
    {
        if (plaque1.isActivated && plaque2.isActivated)
        {
            Destroy(gameObject);
        }
    }
}
