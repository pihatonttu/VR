using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Valon keilan ohjaus
/// </summary>
public class LightConeControl : MonoBehaviour
{
    //Itse nappi ja sen kierto
    [SerializeField]
    GameObject controlNub;
    Vector3 nubRot;
    Vector3 currentRot;

    [SerializeField]
    GameObject condenser;
    [SerializeField]
    float maxHeight = -3.01f;
    [SerializeField]
    float minHeight = -4f;

    [SerializeField]
    float moveSpeed = 1;

    void Start()
    {
        nubRot = controlNub.transform.localEulerAngles;
    }

    void Update()
    {
        //Otetaan kierto
        currentRot = controlNub.transform.localEulerAngles;

        //Jos kierto on muuttunut niin muutetaan se myös pelimaailmaan
        if (currentRot != nubRot)
        {
            if (currentRot.z < nubRot.z && condenser.transform.localPosition.z < maxHeight)
            {
                condenser.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else if (currentRot.z > nubRot.z && condenser.transform.localPosition.z > minHeight)
            {
                condenser.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
            }

            nubRot = currentRot;
        }
    }
}
