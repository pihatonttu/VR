using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

[ExecuteInEditMode]
public class MaskScale : MonoBehaviour
{
    [SerializeField]
    HelpController helpController;
    bool firstTouch = true;
    bool secondTouch = false;
    public float scale = 0.5f;
    float minScale = 0f;
    float maxScale = 2f;
    float shapeTreshold = 0.75f;
    float alphaCutOffUpper = 0.3f;
    float alphaCutOffLower = 0.2f;
    SpriteMask spriteMask;
    float oldRot;

    [SerializeField]
    private Transform RingTransform;
    [SerializeField]
    private CircularDrive RingCircular;


    private void Start()
    {
        spriteMask = GetComponent<SpriteMask>();
        oldRot = RingTransform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        //change scale (size of opening based on diaphram rings rotation.
        float currentRotation = Mathf.Clamp(RingCircular.outAngle/100,minScale,maxScale);

        if (currentRotation != oldRot)
        {
            scale = currentRotation;
            //if (currentRotation < oldRot && scale > minScale)
            //{
            //    scale -= 0.01f;
            //}
            //else if (currentRotation > oldRot && scale < maxScale)
            //{
            //    scale += 0.01f;
            //}

            oldRot = currentRotation;
            
        }

        //scale this masks scale and change rotation
        transform.localScale = new Vector3(scale/10, (scale/10), 1);
        //transform.eulerAngles = new Vector3(0, -90, (scale*10)-5);
        
    }
}
