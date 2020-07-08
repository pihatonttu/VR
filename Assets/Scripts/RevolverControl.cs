using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Objektiivi revolverin ohjaus
/// </summary>
/// 

public class RevolverControl : MonoBehaviour
{
    [SerializeField]
    float noObjectiveAngle;
    [SerializeField]
    float objective10xAngle;
    [SerializeField]
    float objective20xAngle;
    [SerializeField]
    float objective60xAngle;
    [SerializeField]
    float snapLimit;
    [SerializeField]
    bool inLockPosition;
    [SerializeField]
    HelpController helpController;
    [SerializeField]
    //bool isInHand;
    bool firstTouch = true;
    bool secondTouch = false;
    int currentMag;
    
    Vector3 currentRot;

    CircularDrive interactable;

    void Start()
    {        
        inLockPosition = false;
        currentMag = 0;
        interactable = GetComponent<CircularDrive>();
        SnapToClosestObjective(currentRot);
    }

    //-------------------------------------------------
    //Kun käsi aloittaa hoveerauksen niin näytetään ohje
    private void OnHandHoverBegin(Hand hand)
    {
        hand.ShowGrabHint();
    }
    //-------------------------------------------------

    //Kun hoveeraus loppuu niin poistetaan ohje
    private void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
    }
    //---------------------------------------

    private GrabTypes grabbedWithType;
    //Tarkistetaan painalluksia jotta voidaan snäpätä revolveri kun napista päästetään irti
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

        if (grabbedWithType == GrabTypes.None && startingGrabType != GrabTypes.None)
        {
            grabbedWithType = startingGrabType;
            // Triggeriä alettiin painamaan

            hand.HideGrabHint();
        }
        else if (grabbedWithType != GrabTypes.None && isGrabEnding)
        {
            // Triggeristä päästettiin irti
            currentRot = transform.localEulerAngles;
            SnapToClosestObjective(currentRot);

            grabbedWithType = GrabTypes.None;
        }
        
    }
    //-------------------------------------------------

    //Valitse lähin objektiivi
    private void SnapToClosestObjective(Vector3 rotation)
    {
        float closest = FindClosestObjective(rotation);
        transform.localEulerAngles = new Vector3(rotation.x, rotation.y, closest);
        interactable.outAngle = closest;
        inLockPosition = true;
    }

    //Löydä lähin objektiivi
    //TODO: Snäppää aina lähimpään eikä koskaan tyhjään
    private float FindClosestObjective(Vector3 rotation)
    {
        float ret = 0;
        float rot = rotation.z;

        if (calculateAngleDistance(rot,noObjectiveAngle) < snapLimit)
        {
            currentMag = 0;
            ret = noObjectiveAngle;
        }
        else if (calculateAngleDistance(rot, objective10xAngle) < snapLimit)
        {
            currentMag = 10;
            ret = objective10xAngle;
        }
        else if (calculateAngleDistance(rot, objective20xAngle) < snapLimit)
        {
            currentMag = 20;
            ret = objective20xAngle;
        }
        else if (calculateAngleDistance(rot, objective60xAngle) < snapLimit)
        {
            currentMag = 60;
            ret = objective60xAngle;
        }
        else
        {
            currentMag = 0;
            Debug.LogError("No objective found");
        }

        return ret;
    }

    private float calculateAngleDistance(float current,float target)
    {
        return Mathf.Abs(Mathf.DeltaAngle(current, target));
    }

    //Hae tämän hetkinen suurennus
    public int GetCurrentMagnification()
    {
        return currentMag;       
    }

    //Ohjeisiin liittyvä
    public void InHand(bool isHandNow)
    {
        //isInHand = isHandNow;
        //if (firstTouch && currentMag == 10)
        //{
        //    helpController.NextInstruction();
        //    firstTouch = false;
        //    secondTouch = true;
        //}
        //else if (secondTouch && helpController.GetCurrentHelpObject() == this.gameObject)
        //{
        //    helpController.NextInstruction();
        //    secondTouch = false;
        //}
    }

    public void ResetFirstTouch()
    {
        firstTouch = true;
    }

   
}
