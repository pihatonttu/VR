using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ControllerActions : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Action_Boolean teleportAction;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean touchAction; 


   
    // Update is called once per frame
    void Update()
    {
        if (GetTeleportDown())
        {
            Debug.Log($"Teleport {handType}");
        }

        if (GetGrap())
        {
            Debug.Log($"Grap {handType}");
        }

        if (GetTouch())
        {
            Debug.Log($"Touch {handType}");
        }
    }

    public bool GetTeleportDown()
    {
        return teleportAction.GetStateDown(handType);
    }

    public bool GetTouch()
    {
        return touchAction.GetState(handType);
    }
    
    public bool GetGrap()
    {
        return grabAction.GetState(handType);
    }
}
