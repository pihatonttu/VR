using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class TableHandler : MonoBehaviour
{
    [SerializeField]
    GameObject tableMover;
    [SerializeField]
    CircularDrive tableNub;
    private float tableNubRot;
    [SerializeField]
    GameObject holderMover;
    [SerializeField]
    CircularDrive holderNub;
    private float holderNubRot;
    [SerializeField]
    float movespeed;
    float horizontalSpeed;
    [SerializeField]
    Renderer MonitorRenderer;

    float currentRotTN;
    float currentRotHN;
    float offsetY;
    float offsetX;
    float modifierX;
    float modifierY;

    [SerializeField]
    float maxTableY = 0.8f;
    [SerializeField]
    float minTableY = 0.4f;
    [SerializeField]
    float maxHolderX = 2.2f;
    [SerializeField]
    float minHolderX = -0.6f;

    public float OffsetY { get => offsetY; set => offsetY = value; }
    public float OffsetX { get => offsetX; set => offsetX = value; }

    // Start is called before the first frame update
    void Start()
    {
        tableNubRot = tableNub.outAngle;
        holderNubRot = holderNub.outAngle;
        offsetY = Mathf.InverseLerp(maxTableY, minTableY, tableMover.transform.localPosition.y);
        offsetX = Mathf.InverseLerp(minHolderX, maxHolderX, holderMover.transform.localPosition.x);
    }

    // Update is called once per frame
    void Update()
    {
        currentRotTN = tableNub.outAngle;
        currentRotHN = holderNub.outAngle;
        
        if (tableNubRot != currentRotTN)
        {                  
            //Move samplebed when rotating nub
            if (tableNubRot > currentRotTN && tableMover.transform.localPosition.y < maxTableY)
            {
                tableMover.transform.Translate(Vector3.up * movespeed * Time.deltaTime);  
            }
            else if (tableNubRot < currentRotTN && tableMover.transform.localPosition.y > minTableY)
            {
                tableMover.transform.Translate(Vector3.down * movespeed * Time.deltaTime);
            }
            tableNubRot = currentRotTN;

            //Change offset of sample material based on samplebeds position (Y-axis)
            offsetY = Mathf.InverseLerp(maxTableY, minTableY, tableMover.transform.localPosition.y);
            MonitorRenderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX + modifierX, offsetY + modifierY));
        }
        if (holderNubRot != currentRotHN)
        {
            horizontalSpeed = movespeed * 5;
            
            //Move slideholder when rotating nub
            if (holderNubRot > currentRotHN && holderMover.transform.localPosition.x < maxHolderX)
            {
                holderMover.transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);                
            }
            else if (holderNubRot < currentRotHN && holderMover.transform.localPosition.x > minHolderX)
            {
                holderMover.transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);                
            }
            holderNubRot = currentRotHN;

            //Change offset of sample material based on sampleholders position (X-axis)
            offsetX = Mathf.InverseLerp(minHolderX, maxHolderX, holderMover.transform.localPosition.x);
            MonitorRenderer.material.SetTextureOffset("_MainTex", new Vector2(offsetX + modifierX, offsetY + modifierY));
        }
    }

    public void SetOffSet(float x, float y)
    {
        offsetY = Mathf.InverseLerp(maxTableY, minTableY, tableMover.transform.localPosition.y);
        offsetX = Mathf.InverseLerp(minHolderX, maxHolderX, holderMover.transform.localPosition.x);
        modifierX = x;
        modifierY = y;
    }
}
