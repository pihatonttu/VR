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

    //Jotain vanhoja koodeja?
    private float lightSwitchMaxAngle = 20;
    private float lightSwitchMinAngle = 0;
    private Vector3 target;

    //Kuva jonka valoisuutta säädetään
    private SpriteRenderer spriteRenderer;

    //Onko valo päällä
    private bool lightOn;

    //Ohje juttuja
    //[SerializeField]
    //private HelpController helpController;
    private bool firstTouch = true;
    private bool inHand = false;
    
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
        
        ////Valonsäädön napin pyörittelyn säätö
        //Vector3 currentRotation = lightControlNub.transform.localRotation.eulerAngles;        
        //if (currentRotation.z < minAngleLCN - buffer)
        //{            
        //    currentRotation.z = minAngleLCN;
        //}
        //else if (currentRotation.z > maxAngleLCN + buffer)
        //{
        //    currentRotation.z = maxAngleLCN;            
        //}
        
        //lightControlNub.transform.localRotation = Quaternion.Euler(currentRotation);
        


        ////Jos valo on päällä niin säädetään valoisuutta
        //tmp = spriteRenderer.color;
        //if (lightOn)
        //{            
        //    //Lasketaan valon voimakkuus
        //    lightVolume = Mathf.InverseLerp(maxAngleLCN, 0, lightControlNub.transform.localRotation.eulerAngles.z);

        //    //Ohjeisiin liittyvä palikka
        //    if (lightVolume != tmp.a && firstTouch)
        //    {
        //        //Vaihdetaan ohjetta
        //        //helpController.NextInstruction();
        //        Debug.Log("LightControl - NextHelp");
        //        firstTouch = false;
        //    }

        //    //Muunnetaan kuvan valoisuutta
        //    tmp.a = lightVolume;            
        //}

        ////Vaihdetaan väriksi aikaisemmin laskettu väri
        //spriteRenderer.color = tmp;    
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
        {
            //Sammutetaan valo
            tmp.a = 1;
            //Aloitetaan ohje alusta
            //helpController.ResetHelp();
            //Debug.Log("LightControl - ResetHelp");
        }
        //Muutoin sytytetään valo
        else
        {
            tmp.a = currentBrightness;
            
            //helpController.NextInstruction();
            //Debug.Log("HELLo");
        }

        spriteRenderer.color = tmp;
        lightOn = _lightOn;
    }
}
