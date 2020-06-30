using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Käännetään okulaareja
/// </summary>
public class OcularRotator : MonoBehaviour
{
    [SerializeField]
    private float movespeed;
    [SerializeField]
    private OcularRotator otherOcular;
    private bool rotUp = false;
    private bool rotDown = false;
    private bool firstTouch = true;
    [SerializeField]
    private HelpController helpController;

    
    // Update is called once per frame    
    void Update()
    {
        if (rotUp)
        {
            RotateUp();
        }
        else if (rotDown)
        {
            RotateDown();
        }
    }

    private void RotateUp()
    {
        transform.Rotate(Vector3.up * movespeed * Time.deltaTime);
        //Debug.Log("Rotating Right");
    }

    private void RotateDown()
    {
        transform.Rotate(Vector3.down * movespeed * Time.deltaTime);
        //Debug.Log("Rotating Left");

    }

    public void Rotate(string direction)
    {
        if (firstTouch)
        {
            otherOcular.firstTouchUsed();
            firstTouch = false;
            helpController.NextInstruction();
            //Debug.Log($"OcularRotator{this.gameObject.name} - NextHelp");
                       
        }
        if (direction == "UP")
        {
            rotUp = true;
            rotDown = false;
        }
        else if (direction == "DOWN")
        {
            rotDown = true;
            rotUp = false;
        }
        else if (direction == "NONE")
        {
            rotUp = rotDown = false;
        }
    }

    public void firstTouchUsed()
    {
        firstTouch = false;
    }
}
