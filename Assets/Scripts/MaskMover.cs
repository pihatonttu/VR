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
    LinearMapping xNub;
    private float xNubRot;

    [SerializeField]
    LinearMapping yNub;
    private float yNubRot;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    HelpController helpController;

    private float buffer = 0.05f;
    private bool firstTouch = true;

    void Start()
    {
        //Otetaan nykyinen asento talteen
        xNubRot = xNub.value;
        yNubRot = yNub.value;
    }

    void Update()
    {
        //Otetaan asento
        float xRot = xNub.value;
        float yRot = yNub.value;

        //Tarkastetaan miten se on muuttunut
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

        //Ohjeisiin liittyvä
        if (transform.localPosition.x < buffer && transform.localPosition.x > -buffer && transform.localPosition.y < buffer && transform.localPosition.y > -buffer && firstTouch)
        {
            firstTouch = false;
            helpController.NextInstruction();
        }
    }
}
