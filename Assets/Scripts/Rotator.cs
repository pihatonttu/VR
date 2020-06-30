using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    float movespeed;

    bool rotRight = false;
    bool rotLeft = false;

    void LateUpdate()
    {
        if (rotRight)
        {
            RotateRight();
        }
        else if (rotLeft)
        {
            RotateLeft();
        }
    }

    private void RotateRight()
    {
        transform.Rotate(Vector3.back * movespeed * Time.deltaTime);
        //Debug.Log("Rotating Right");
    }

    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward * movespeed * Time.deltaTime);
        //Debug.Log("Rotating Left");
        
    }

    public void Rotate(string direction)
    {
        if (direction == "RIGHT")
        {
            rotRight = true;
            rotLeft = false;
        }
        else if (direction == "LEFT")
        {
            rotLeft = true;
            rotRight = false;
        }
        else if (direction == "NONE")
        {
            rotLeft = rotRight = false;
        }
    }
}
