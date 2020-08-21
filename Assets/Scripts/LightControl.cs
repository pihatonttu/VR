using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Valoisuuden säätö
/// </summary>
public class LightControl : MonoBehaviour
{
    //Valonsäätö nappi
    [SerializeField]
    private LinearMapping lightControlNub;

    //Nykyinen kirkkaus
    [SerializeField]
    private float currentBrightness;

    //Apuväri
    private Color tmp;

    private int buffer = 1;

    //Kuva jonka valoisuutta säädetään
    private SpriteRenderer spriteRenderer;

    //Onko valo päällä
    private bool lightOn;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (lightControlNub.value != currentBrightness)
        {
            //Nykyinen kirkkaus
            currentBrightness = Mathf.Abs(lightControlNub.value - 1);

            ChangeBrightness(currentBrightness);
        }
         
    }

    public void ChangeBrightness(float brightness)
    {
        if (lightOn)
        {
            tmp = spriteRenderer.color;
            tmp.a = Mathf.Min(0.99f, brightness);
            spriteRenderer.color = tmp;
        }
    }

    //Valon sammuttaminen ja päälle laitto
    public void ToggleLight(bool _lightOn)
    {
        //Jos valo ei ole päällä niin sytytetään se
        if (!_lightOn)
            tmp.a = 1;
        else//Muutoin sytytetään valo
            tmp.a = currentBrightness;

        spriteRenderer.color = tmp;
        lightOn = _lightOn;
    }
}
