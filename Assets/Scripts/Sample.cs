using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    //sample objects should have image on magnification levels of 10x, 20x, 30x and starting offset for material.
    [SerializeField]
    Texture mag10;
    [SerializeField]
    Texture mag20;
    [SerializeField]
    Texture mag60;
    [SerializeField]
    Vector2 offsetStart;    

    public Texture Mag60 { get => mag60; set => mag60 = value; }
    public Texture Mag20 { get => mag20; set => mag20 = value; }
    public Texture Mag10 { get => mag10; set => mag10 = value; }
    public Vector2 OffsetStart { get => offsetStart; set => offsetStart = value; }
}
