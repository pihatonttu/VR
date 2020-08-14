using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTutorial : MonoBehaviour
{
    //Näyteen paikka
    [SerializeField]
    Transform samplePlace;

    //Korostus objekti
    [SerializeField]
    GameObject highlightSpot;

    //Onko paikalla jo jotain
    bool inPlace = false;

    //onko näyte on snapzonen sisäpuolella
    private bool insideSnapZone;

    //Nykyinen näyte joka on alueella
    [SerializeField]
    GameObject currentSample;

    // Start is called before the first frame update
    void Start()
    {
        //Haetaan paikka näytteelle
        samplePlace = this.gameObject.transform.GetChild(0);
        //Haetaan korostus objekti
        highlightSpot = samplePlace.GetChild(0).gameObject;
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
                currentSample = null;
                //currentSample.GetComponent<Rigidbody>().isKinematic = false;
                inPlace = false;
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
            //currentSample.GetComponent<Rigidbody>().isKinematic = true;
        }

        insideSnapZone = false;
        highlightSpot.SetActive(false);
    }

}
