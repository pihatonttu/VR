using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/// <summary>
/// Eri objektien interaktio ohjaimilla
/// </summary>
public class ObjectInterAction : MonoBehaviour
{
    private const float ZERO = 0;
    private const string NONE = "NONE";
    private const string UP = "UP";
    private const string DOWN = "DOWN";
    private const string RIGHT = "RIGHT";
    private const string LEFT = "LEFT";

    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean grabAction;
    public SteamVR_Action_Boolean touchAction;    

    private GameObject collidingObject;
    private GameObject objectInHand = null;
    private Vector3 origin;
    private float timeOffSet = 0.1f;
    private float moveBuffer = 0.02f;
    private float highlightWidth = 4;

    
    [SerializeField]
    private Color highlightColor; //Korostusväri
    Color tmpcolor = Color.clear; //Tallennetaan tähän vanha väri tarvittaessa

    private GameObject currentInteract;

    void Update()
    {        
        //Jos ohjaimella otetaan kiinni jostain
        if (grabAction.GetLastStateDown(handType))
        {            
            //ja se ei ole untagged
            if (collidingObject && collidingObject.tag != "Untagged")
            {               
                //jos se on holder niin näytetään slideholderin animaatio
                if (collidingObject.tag == "holder")
                {
                    collidingObject.GetComponent<SlideHolder>().PlayAnimation();                    
                }
                //Jos se on switch niin laitetaan valot päälle
                else if (collidingObject.tag == "switch")
                {
                    collidingObject.GetComponent<LightToggler>().ToggleLight();
                    Debug.Log("Turn on/off");
                }     
                //Muuten otetaan kiinni esineestä
                else
                {
                    GrabObject();
                }
            }            
        }

        
        //Jos ohjaimella päästetään irti
        if (grabAction.GetLastStateUp(handType))
        {
            //Päästetään esineestä irti jos jotain on kädessä
            if (objectInHand)
            {
                ReleaseObject();
            }
            //Nollataan muiden kääntyminen että ne ei enää käänny kun on päästetty irti
            else if (collidingObject && collidingObject.GetComponent<Rotator>())
            {
                collidingObject.GetComponent<Rotator>().Rotate(NONE);                
            }
            else if (collidingObject && collidingObject.tag == "ocular")
            {
                collidingObject.GetComponent<OcularRotator>().Rotate(NONE);
            }
        }

        //Jos kädessä on esine
        if (objectInHand)
        {
            //Jos se on rotator
            if (objectInHand.GetComponent<Rotator>())
            {
                RotateRotator();
            }
            else if (objectInHand.tag == "ocular")
            {
                RotateOcular();
            }
            //Jos se on muutakuin liikuteltava esine
            else if (objectInHand.tag != "movable")
            {
                SetRotation(objectInHand, Quaternion.Euler(origin.x, origin.y, objectInHand.transform.rotation.eulerAngles.z));
            }
        }

        //Jos törmätään johonkin
        //if (collidingObject)
        //{
        //    //Tarkastetaan onko sillä outline ja se ei ole jo aktiivinen
        //    if (collidingObject.GetComponent<Outline>() && collidingObject != currentInteract)
        //    {
        //        //Poistetaan edellisestä korostus
        //        if (currentInteract != null) UnHighlightObject(currentInteract);

        //        //Tehdään uudesta törmäyksestä nykyinen
        //        if (collidingObject != null) currentInteract = collidingObject;

        //        //Korostetaan se
        //        HighlightObject(collidingObject);

        //        //Debug.Log("Värjätään palikkaa");

        //    }
        //}
        //if (!collidingObject)
        //{
        //    if (currentInteract != null) 
        //    {
        //        UnHighlightObject(currentInteract);
        //        tmpcolor = Color.clear;
        //    }
            
        //}
    }

    private void SetCollidingObject(Collider col)
    {        
        collidingObject = col.gameObject;        
    }
    
    //Tarkistetaan mikä esine on törmäyksen kohteena
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag != "Untagged")
        {
            if (other.gameObject != collidingObject)
            {
                //Jos aikaisempi on olemassa niin poistetaan korostus
                if (collidingObject) UnHighlightObject(collidingObject);

                //Tehdään uusi nykyinen objekti
                SetCollidingObject(other);

                //Jos uudessa objektissa on outline niin käytetään sitä
                if (collidingObject) HighlightObject(collidingObject);
            }

        }

    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.tag != "Untagged")
        //{
        //    SetCollidingObject(other);
        //}        
    }

    //Kun poistutaan trigger törmäyksistä
    private void OnTriggerExit(Collider other)
    {
        //Jos ei olla törmäyksen kohdetta ei ole niin poistutaan
        if (!collidingObject)
            return;

        if (collidingObject.GetComponent<Outline>()) UnHighlightObject(collidingObject);

        //Jos törmäyksen kohteella on Rotator komponentti niin pysäytetään se
        if (collidingObject && collidingObject.GetComponent<Rotator>())
        {            
            collidingObject.GetComponent<Rotator>().Rotate(NONE);
        }

        //Jos törmäyksen kohteena on lasit niin lopetetaan pyörittäminen
        if (collidingObject && collidingObject.tag == "ocular")
        {
            collidingObject.GetComponent<OcularRotator>().Rotate(NONE);
        }

        //Jos törmäyksen kohde on sama kuin kädessä oleva niin pudotetaan se?
        if (objectInHand && objectInHand.tag == other.tag)
        {
            ReleaseObject();            
        }

        ////Jos törmäyksen kohteella on Outline komponentti niin nollataan se
        //if (collidingObject.GetComponent<Outline>())
        //{
        //    UnHighlightObject(collidingObject);
        //}

        //Poistetaan törmäysobjekti
        tmpcolor = Color.clear;
        collidingObject = null;
    }

    private FixedJoint AddFixedJoint()
    {
        FixedJoint fx = gameObject.AddComponent<FixedJoint>();
        fx.breakForce = 10000;
        fx.breakTorque = 10000;
        return fx;
    }

    //Tartutaan esineeseen
    private void GrabObject()
    {   
        //Esine kädessä on sama kuin ohjaimen kanssa törmännyt esine
        objectInHand = collidingObject;
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();
        collidingObject = null;               

        //Jos esine on sellainen jota ei voida ottaa käteen niin palataan
        if (objectInHand.GetComponent<Rotator>() || objectInHand.tag == "ocular")
            return;
        
        //Jos esine on sellanen jota ei voida ottaa käteen mutta sitä voidaan pyöritellä
        if (objectInHand.tag != "movable")
        {
            //Poistetaan kinematiikka
            rb.isKinematic = false;
            //Otetaan alkuperäinen kierto
            origin = objectInHand.transform.rotation.eulerAngles;

            //Jos se on objektiivirevolveri
            if (objectInHand.tag == "revolver")
            {
                objectInHand.GetComponent<RevolverControl>().InHand(true);
            }
            //Jos se on näytelasin pitelijä
            else if (objectInHand.tag == "holder")
            {
                objectInHand.GetComponent<SlideHolder>().InHand(true);                
            }
        }       
        
        //Lisätään oma sarana
        FixedJoint joint = AddFixedJoint();
        joint.connectedBody = rb;
        
        //Debug.Log($"Grabbing: {objectInHand.tag}");
    }

    //Korosta objekti
    private void HighlightObject(GameObject touchedObject)
    {
        if (touchedObject.GetComponent<Outline>())
        {
            if (touchedObject.GetComponent<Outline>().OutlineWidth > 0)
                if(touchedObject.GetComponent<Outline>().OutlineColor != highlightColor && touchedObject.GetComponent<Outline>().OutlineColor != Color.clear)
                    tmpcolor = touchedObject.GetComponent<Outline>().OutlineColor;

            touchedObject.GetComponent<Outline>().OutlineWidth = highlightWidth;
            touchedObject.GetComponent<Outline>().OutlineColor = highlightColor;
        }
    }

    //Poistetaan esineen korostus tai palautetaan takaisin aikasempi jos sellainen oli
    private void UnHighlightObject(GameObject touchedObject)
    {
        if (touchedObject.GetComponent<Outline>())
        {
            if (tmpcolor != Color.clear && tmpcolor != highlightColor)
            {
                touchedObject.GetComponent<Outline>().OutlineColor = tmpcolor;
            }
            else
            {
                touchedObject.GetComponent<Outline>().OutlineWidth = ZERO;
            }
        }
    }

    //Päästetään irti objektista
    private void ReleaseObject()
    {
        //Jos objektia ei ole kädessä niin palataan
        if (objectInHand == null)
            return;

        //Jos objekti sisältää Rotator komponentin niin pysäytetään se
        if (objectInHand.GetComponent<Rotator>())
        {            
            objectInHand.GetComponent<Rotator>().Rotate(NONE);
            objectInHand = null;
            return;
        }
        //Jos objekti sisältää ocular tagin niin pysäytetään se
        else if (objectInHand.tag == "ocular")
        {
            objectInHand.GetComponent<OcularRotator>().Rotate(NONE);
            objectInHand = null;
            return;
        }

        //Jos ylemmät ei toteudu niin otetaan esineen rigidbody
        Rigidbody rb = objectInHand.GetComponent<Rigidbody>();

        //Tuhotaan nykyinen sarana
        GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());

        //Jos se on muutakuin movable 
        if (objectInHand.tag != "movable")
        {
            //Nollataan kaikki
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            //Tehdään siitä taas liikkumaton objekti
            rb.isKinematic = true;
            SetRotation(objectInHand, Quaternion.Euler(origin.x, origin.y, objectInHand.transform.rotation.eulerAngles.z));            

            //Poistetaan InHand tieto esineistä
            if (objectInHand.tag == "revolver")
            {
                objectInHand.GetComponent<RevolverControl>().InHand(false);
            }
            else if (objectInHand.tag == "holder")
            {
                objectInHand.GetComponent<SlideHolder>().InHand(false);
            }
        }        
        else //Jos se on tavallinen movable esine
        {        
            rb.velocity = controllerPose.GetVelocity();
            rb.angularVelocity = controllerPose.GetAngularVelocity();
        }

        //Nollataan tiedot
        UnHighlightObject(objectInHand);
        objectInHand = null;
        rb = null;        
    }

    private void RotateRotator()
    {
        Rotator rotator = objectInHand.GetComponent<Rotator>();
        controllerPose.GetVelocitiesAtTimeOffset(timeOffSet, out Vector3 predictedVelocity, out Vector3 predictedAngularVelocity);

        if (predictedVelocity.x > moveBuffer)
        {
            rotator.Rotate(RIGHT);
        }
        else if (predictedVelocity.x < -moveBuffer)
        {
            rotator.Rotate(LEFT);
        }
    }

    private void RotateOcular()
    {
        OcularRotator rotator = objectInHand.GetComponent<OcularRotator>();
        controllerPose.GetVelocitiesAtTimeOffset(timeOffSet, out Vector3 predictedVelocity, out Vector3 predictedAngularVelocity);

        if (predictedVelocity.y > moveBuffer)
        {
            rotator.Rotate(UP);
        }
        else if (predictedVelocity.y < -moveBuffer)
        {
            rotator.Rotate(DOWN);
        }
    }

    private void SetRotation(GameObject objectToSet, Quaternion targetRot)
    {
        objectToSet.transform.rotation = targetRot;        
        //Debug.Log("Reset Done");
    }
    
    

}
