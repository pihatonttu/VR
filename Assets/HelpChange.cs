using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpChange : MonoBehaviour
{
    [SerializeField]
    private HelpController helpController;


    private void OnTriggerEnter(Collider other)
    {
        helpController.NextInstruction();
    }
}
