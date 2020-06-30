using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideChecker : MonoBehaviour
{
    //Triggerit
    [SerializeField]
    GameObject holderSolidTarget;
    bool hstHit;
    [SerializeField]
    GameObject sampleTrigger;
    bool hltHit;

    //Onko näyte paikallaan
    bool inPlace = false;

    //Näytteenpidin
    [SerializeField]
    SlideHolder slideHolder;

    //Näyttö
    [SerializeField]
    Renderer MonitorRenderer;

    //Näyte
    Sample sample;

    //Tyhjä kuva jos näytettä ei ole
    [SerializeField]
    Texture empty;

    [SerializeField]
    TableHandler table;
    [SerializeField]
    Transform slidePlace;
    Vector3 normalPosition;

    [SerializeField]
    RevolverControl objectiveRevolver;
    int currentMag = 0;


    private void Start()
    {        
        sample = GetComponent<Sample>();
        normalPosition = slidePlace.transform.position;
    }

    void Update()
    {        
        //Jos näyte on paikallaan ja objektiivi on vaihdettu
        if (inPlace && currentMag != objectiveRevolver.GetCurrentMagnification())
        {
            ChangeSampleImage();
        }
    }     

    //Kun näyte laitetaan paikalleen
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == holderSolidTarget)
        {
            hstHit = true;           
        }
        if (other.gameObject == sampleTrigger)
        {
            hltHit = true;            
        }
        CheckSituation();
    }

    //Kun näyte poistuu paikaltaan
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == sampleTrigger)
        {
            hltHit = false;
        }
        if (other.gameObject == holderSolidTarget)
        {
            hstHit = false;
        }
        CheckSituation();
    }

    //Haetaan oikea kuva suurennuksen mukaan
    private Texture GetSampleImage(int magnification)
    {
        if (magnification == 10)
        {
            currentMag = 10;
            return sample.Mag10;
        }
        else if (magnification == 20)
        {
            currentMag = 20;
            return sample.Mag20;
        }
        else if (magnification == 60)
        {
            currentMag = 60;
            return sample.Mag60;
        }
        else
        {
            currentMag = 0;
            return empty;
        }
    }


    //Tarkistetaan mikä tilanne näytelevyllä
    private void CheckSituation()
    {
        //Jos näyte on laitettu paikalleen
        if (hstHit && hltHit && !inPlace)
        {
            slideHolder.SlideAtPlace(true);
            inPlace = true;
            transform.localPosition = normalPosition;
            transform.SetParent(slidePlace);
            transform.localRotation = Quaternion.identity;

            //Vaihdetaan näytteen kuva objektiivin mukaan
            ChangeSampleImage();
        }

        //Jos näyte otetaan pois paikaltaan
        else if (inPlace && !hltHit || !hstHit)
        {
            RemoveSample();               
        }        
    }

    //Vaihdetaan kuva suurennuksen mukaan
    public void ChangeSampleImage()
    {
        MonitorRenderer.material.SetTexture("_MainTex", GetSampleImage(objectiveRevolver.GetCurrentMagnification()));
        MonitorRenderer.material.SetTextureOffset("_MainTex", new Vector2(table.OffsetX + sample.OffsetStart.x, table.OffsetY + sample.OffsetStart.y));        
        table.SetOffSet(sample.OffsetStart.x, sample.OffsetStart.y);
    }

    //Poistetaan näytteen tiedot
    public void RemoveSample()
    {
        slideHolder.SlideAtPlace(false);
        inPlace = false;
        transform.SetParent(null);
        MonitorRenderer.material.SetTexture("_MainTex", empty);
    }
}
