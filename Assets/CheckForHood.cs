using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForHood : MonoBehaviour
{
    public bool IsHoodOn = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hood")
        {
            IsHoodOn = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Hood")
        {
            IsHoodOn = false;
        }
    }
}
