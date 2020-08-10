using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class SamplePlaceTrigger : MonoBehaviour
{
    //Näyteen paikka
    Transform samplePlace;  
    //Korostus objekti
    GameObject highlightSpot;   

    //Onko paikalla jo jotain
    bool inPlace = false;   
    //onko näyte on snapzonen sisäpuolella
    private bool insideSnapZone;

    //Näytteenpidin
    [SerializeField]
    GameObject slideHolder;

    [SerializeField]
    TableHandler table;

    //Kuinka paljon näyteenpitimen pitää kääntyä tietyssä tilanteessa
    const float noSampleRot = 359f;
    const float sampleInPlaceRot = 335f;

    //Nykyinen näyte joka on alueella
    [SerializeField]
    GameObject currentSample;

    //Näyttö
    [SerializeField]
    Renderer MonitorRenderer;

    //Objektiivit
    [SerializeField]
    RevolverControl objectiveRevolver;
    //Nykyinen tarkkuus
    int currentMag = 0;

    [SerializeField]
    Texture emptyImage;

    private HelpController tutorial;

    private void Awake()
    {
        tutorial = FindObjectOfType<HelpController>();
    }

    private void Start()
    {
        //Haetaan paikka näytteelle
        samplePlace = this.gameObject.transform.GetChild(0);
        //Haetaan korostus objekti
        highlightSpot = samplePlace.GetChild(0).gameObject;
    }

    private void Update()
    {
        //Jos näyte on paikallaan ja objektiivi on vaihdettu
        if (inPlace && currentMag != objectiveRevolver.GetCurrentMagnification())
        {
            ChangeMonitorImage();
        }
    }

    //Kun näyte osuu triggeriin otetaan sen tiedot talteen ja näytetään reunaviivoja sen paikalle
    private void OnTriggerEnter(Collider other)
    {
        //Tehdään vain jos paikalla ei ole mitään vielä
        if (!inPlace && other.gameObject.tag == "Sample")
        {
            insideSnapZone = true;
            currentSample = other.gameObject;
            highlightSpot.SetActive(true);
        }
    }

    //Kun näyte poistuu triggeristä niin tyhjennetään kaikki
    private void OnTriggerExit(Collider other)
    {
        //Jos nykyinen näyte on muutakuin tyhjä
        if (currentSample != null)
        {
            //Jos nykyinen näyte on sama kuin triggeristä poistuva esine
            if (currentSample.gameObject == other.gameObject)
            {
                currentSample.transform.parent = null;
                currentSample = null;
                inPlace = false;
                setSlideHolder();
                MonitorRenderer.material.SetTexture("_MainTex", emptyImage);
            }
        }

        insideSnapZone = false;      
        highlightSpot.SetActive(false);   
    }

    //Tarkistetaan ollaanko snappi alueella silloinkuin tämä ajetaan (ajetaan näytteen onDetachista)
    public void CheckForSnapZone()
    {
        if (insideSnapZone && currentSample != null && !inPlace)
        {
            inPlace = true;
            currentSample.transform.position = samplePlace.position;
            currentSample.transform.rotation = samplePlace.rotation;
            currentSample.transform.SetParent(samplePlace);

            //Siirretään näytteenpidin oikein paikalleen
            setSlideHolder();

            //laitetaan kuva näytölle
            ChangeMonitorImage();

        }

        insideSnapZone = false;
        highlightSpot.SetActive(false);
    }

    //Säädetään näytteenpidintä sen mukaan onko paikalla mitään
    private void setSlideHolder()
    {
        if (inPlace)
        {
            slideHolder.transform.localRotation = Quaternion.Euler(0, 0, sampleInPlaceRot);
        } else
        {
            slideHolder.transform.localRotation = Quaternion.Euler(0, 0, noSampleRot);
        }
    }

    private Texture GetSampleImage(int magnification)
    {
        if (magnification == 10)
        {
            currentMag = 10;
            return currentSample.GetComponent<Sample>().Mag10;
        }
        else if (magnification == 20)
        {
            currentMag = 20;
            return currentSample.GetComponent<Sample>().Mag20;
        }
        else if (magnification == 60)
        {
            currentMag = 60;
            return currentSample.GetComponent<Sample>().Mag60;
        }
        else
        {
            currentMag = 0;
            return emptyImage;
        }
    }

    public void ChangeMonitorImage()
    {
        if (inPlace)
        {
            MonitorRenderer.material.SetTexture("_MainTex", GetSampleImage(objectiveRevolver.GetCurrentMagnification()));
            //MonitorRenderer.material.SetTextureOffset("_MainTex", new Vector2(table.OffsetX + currentSample.GetComponent<Sample>().OffsetStart.x, table.OffsetY + currentSample.GetComponent<Sample>().OffsetStart.y));
            table.SetOffSet(currentSample.GetComponent<Sample>().OffsetStart.x, currentSample.GetComponent<Sample>().OffsetStart.y);
        }
    }
    public bool IsInPlace()
    {
        return inPlace;
    }

}
