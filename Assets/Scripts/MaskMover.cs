using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Tällä Siirretään kenttähimmennintä
/// </summary>
public class MaskMover : MonoBehaviour
{
    [SerializeField]
    CircularDrive xNub;
    private float xNubRot;

    [SerializeField]
    CircularDrive yNub;
    private float yNubRot;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    HelpController helpController;

    void Start()
    {
        //Otetaan nykyinen asento talteen
        xNubRot = xNub.outAngle;
        yNubRot = yNub.outAngle;
    }

    void Update()
    {
        //Otetaan asento
        float xRot = xNub.outAngle;
        float yRot = yNub.outAngle;

        if (xRot > xNubRot)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
        else if (xRot < xNubRot)
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        xNubRot = xRot;

        if (yRot > yNubRot)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }
        else if (yRot < yNubRot)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
        yNubRot = yRot;

    }
}
