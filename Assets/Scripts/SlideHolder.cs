using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideHolder : MonoBehaviour
{
    [SerializeField]
    bool SampleInPlace;
    [SerializeField]
    bool atTarget = false;
    float noSampleRot = 359f;    
    float sampleInPlaceRot = 335f;    
    [SerializeField]
    float rotationSpeed;
    bool isInHand = false;
    float buffer;

    [SerializeField]
    private HelpController helpController;
    private bool firstTouch = true;
    

    // Update is called once per frame
    void Update()
    {        
        if (!isInHand)
        {            
            if (!atTarget)
            {
                RotateHolder(SampleInPlace);
            }            
        }        
    }    

    private void RotateHolder (bool slideIsInPlace)
    {
        GetComponent<Animator>().enabled = false;
        Vector3 currentRotation = transform.localRotation.eulerAngles;
        float angle;
        if (slideIsInPlace)
        {
            angle = sampleInPlaceRot;
            if (firstTouch)
            {
                helpController.NextInstruction();
                firstTouch = false;
                Debug.Log("SlideHolder - NextHelp");
            }
            
        }
        else
        {
            angle = noSampleRot;
        }

        if(currentRotation.z < angle - buffer)
        {            
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);            
        }
        else
        {
            atTarget = true;            
        }
        //Debug.Log("Rotating holder");
    }

    public void SlideAtPlace(bool isInPlace)
    {
        SampleInPlace = isInPlace;
    }

    public void InHand(bool inHandNow)
    {
        isInHand = inHandNow;
        atTarget = inHandNow;
    }

    public void PlayAnimation()
    {
        string animation;
        Animator animator = GetComponent<Animator>();
        if (animator.enabled)
        {
            animator.enabled = false;
            atTarget = false;
        }
        else
        {
            animator.enabled = true;
            if (SampleInPlace)
            {
                animation = "SampleInPlaceAni";
                
                Debug.Log($"Next Ani, sampleinPlace: {SampleInPlace}");
            }
            else
            {
                animation = "NoSlideAni";
                Debug.Log("No slide animation");
            }
            animator.Play(animation,0,0f);
            Debug.Log($"Animating, sampleON: {SampleInPlace}");
        }
    }
}
