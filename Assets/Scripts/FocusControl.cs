using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Tarkkuuden säätö
/// </summary>
public class FocusControl : MonoBehaviour
{
    //Karkeasäätö napin kierto
    [SerializeField]
    Transform coarsePivot;
    Vector3 coarsePivotRot;

    [SerializeField]
    LinearMapping coarseLinearMapping;
    float currentCoarseRot;

    [SerializeField]
    LinearMapping fineLinearMapping;
    float currentFineRot;


    //Hienosäätö napin kierto
    [SerializeField]
    Transform finePivot;
    Vector3 finePivotRot;

    //Nappejen nykyinen kierto
    Vector3 currentCoarsePivotRot;
    Vector3 currentFinePivotRot;

    //Karkeasäätö napin pyörimisnopeus
    [SerializeField]
    float coarseSpeed;

    //Hienosäätö napin pyörimisnopeus
    [SerializeField]
    float fineSpeed;

    //Pöydän maksimit ja minimit
    float maxHeight = 4.5f;
    [SerializeField]
    float minHeight = 1f;

    //Kohta jossa objetiivi ja lasi osuvat toisiinsa
    [SerializeField]
    float ContactHeight = 4.159237f;

    //Ohjeet
    [SerializeField]
    HelpController helpController;

    [SerializeField]
    Material blurMat;
    float radius = 1f;    
    [SerializeField]
    float coarseFocusSpeed = 200f;
    [SerializeField]
    float fineFocusSpeed = 20f;
    float MinCoarseFocus = 1f;
    float MinFocus = 0f;
    float MaxFocus = 10f;
    float buffer = 0.2f;
    float tableTarget = 3.857f;
    bool firstTouch = true;

    Vector3 oldPos;
    Vector3 tableTransform;
    

    void Start()
    {
        currentCoarseRot = coarseLinearMapping.value;
        currentFineRot = fineLinearMapping.value;

        //Otetaan alkuasennot napeista ja pöydästä
        //coarsePivotRot = coarsePivot.localEulerAngles;
        //finePivotRot = finePivot.localEulerAngles;
        tableTransform = oldPos = transform.localPosition;

        //Asetetaan alku sumennus
        radius = Mathf.Lerp(minHeight, tableTarget, transform.position.z);
        blurMat.SetFloat("_Radius", radius);
    }

    void Update()
    {
        //Tarkitetaan onko karkeasäädön arvo muuttunut
        if (coarseLinearMapping.value != currentCoarseRot)
        {
            //Jos se on suurempi kuin aikaisemmin niin nostetaan pöytää
            //Ja jos se on pienempi niin lasketaan pöytää
            if (currentCoarseRot > coarseLinearMapping.value)
                RaiseTable(coarseSpeed);
            else
                LowerTable(coarseSpeed);

            //Otetaan talteen muuttujaan
            currentCoarseRot = coarseLinearMapping.value;
            //Asetetaan tarkkuus
            SetFocus(coarseFocusSpeed, MinCoarseFocus);
        }

        //Tarkistetaan onko hienosäädön arvo muuttunut
        if (fineLinearMapping.value != currentFineRot)
        {
            if (currentFineRot > fineLinearMapping.value)
                RaiseTable(fineSpeed);
            else
                LowerTable(fineSpeed);

            //Otetaan talteen muuttujaan
            currentFineRot = fineLinearMapping.value;
            //Asetetaan tarkkuus
            SetFocus(fineFocusSpeed, MinFocus);
        }

        //Otetaan nykyiseksi kierroksi viimeisin kierto asento
        //currentCoarsePivotRot = coarsePivot.localRotation.eulerAngles;
        //currentFinePivotRot = finePivot.localRotation.eulerAngles;

        ////Jos kierto on muuttunut käännetään nappia
        //if (currentCoarsePivotRot.z != coarsePivotRot.z)
        //{           
        //    RotatePivot(ref currentCoarsePivotRot, ref coarsePivotRot,ref coarsePivot, coarseSpeed);
        //    SetFocus(coarseFocusSpeed, MinCoarseFocus);    
        //}
        //if (currentFinePivotRot.z != finePivotRot.z)
        //{
        //    RotatePivot(ref currentFinePivotRot, ref finePivotRot, ref finePivot, fineSpeed);            
        //    SetFocus(fineFocusSpeed, MinFocus); 
        //}

        //Debug.Log(tableTransform.z);
    }

    /// <summary>
    /// Napin kääntämistä ja pöydän nostoa
    /// </summary>
    /// <param name="currentRot">Nykyinen kierto</param>
    /// <param name="pivotRot">Uusi kierto</param>
    /// <param name="pivot">Nappi</param>
    /// <param name="speed">Pyörimisnopeus</param>
    private void RotatePivot(ref Vector3 currentRot, ref Vector3 pivotRot, ref Transform pivot, float speed)
    {
        //Jos nykyinen kierto on suurempi kuin uusi kierto
        if (currentRot.z > pivotRot.z)
        {    
            RaiseTable(speed);//Nostetaan pöytää
            CheckIfObjectiveContact();//Tarkistetaan osuuko lasi objektiiviin
        }
        //Jos taas toisinpäin
        else if (currentRot.z < pivotRot.z)
        {
            LowerTable(speed); //Lasketaan pöytää
        }
        
        //Päivitetään napin kierto
        //pivot.localEulerAngles = currentRot;
        //pivotRot = currentRot;
    }

    /// <summary>
    /// Tarkistetaan osuuko objektiivi lasiin
    /// </summary>
    public void CheckIfObjectiveContact()
    {
        if (transform.localPosition.z >= ContactHeight)
        {
            Debug.Log("Contact, brake sample");
            //TODO:
            //Break sample and give minus points.
        }
    }

    /// <summary>
    /// Nostetaan ristisiirtopöytää
    /// </summary>
    /// <param name="speed">Nopeus</param>
    private void RaiseTable(float speed)
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);        
    }

    /// <summary>
    /// Lasketaan ristisiirtopöytää
    /// </summary>
    /// <param name="speed">Nopeus</param>
    private void LowerTable(float speed)
    {
        transform.Translate(Vector3.back * speed * Time.deltaTime);
    }

    /// <summary>
    /// Lisätään tarkkuutta DEBUGGIA VARTEN
    /// </summary>
    /// <param name="speed">Nopeus</param>
    private void AddFocus(float speed)
    {        
        radius -= speed * Time.deltaTime;
    }

    /// <summary>
    /// Pienennetään tarkkuutta DEBUGGIA VARTEN
    /// </summary>
    /// <param name="speed">Nopeus</param>
    private void DecreaseFocus(float speed)
    {
        radius += speed * Time.deltaTime;
    }

    /// <summary>
    /// Asetetaan tarkkuus
    /// </summary>
    /// <param name="speed">Nopeus</param>
    /// <param name="target">Kohde</param>
    private void SetFocus(float speed, float target) 
    {
        //Ristisiirtopöydän sijainti
        Vector3 tablePos = transform.localPosition;

        //Jos pöydän korkeus on pienempi kuin maksimi ja pöydän korkeus on suurempi kuin minimi
        if (tablePos.z < maxHeight && tablePos.z > minHeight)
        {
            //Jos pöydän korkeus on suurempi kuin tavoite niin sumennetaan kuvaa
            if (tablePos.z > tableTarget)
            {
                radius = Mathf.InverseLerp(tableTarget, maxHeight, tablePos.z) * 5;
            }
            else//Jos pienempi niin sumennetaan myös
            {
                radius = Mathf.InverseLerp(tableTarget, minHeight, tablePos.z) * 5;
            }
        }
        //Jos pöytä ylittää maksimikorkeuden
        if (tablePos.z > maxHeight)
        {
            //Annetaan uusi sijainti
            tablePos = new Vector3(tablePos.x, tablePos.y, maxHeight);
        }
        //Jos pöytä alittaa minimikorkeuden
        else if (tablePos.z < minHeight)
        {
            //Annetaan uusi sijainti
            tablePos = new Vector3(tablePos.x, tablePos.y, minHeight);
        }

        //Annetaan pöydälle uusi sijainti
        transform.localPosition = tablePos;

        //Annetaan haluttu sumennus
        blurMat.SetFloat("_Radius", radius);

        //Jos nappia on liikutettu ekan kerran sopivasti niin vaihdetaan ohjetta
        if (radius <= buffer && firstTouch)
        {
            helpController.NextInstruction();
            firstTouch = false;
        }


        //Debug.Log($"Focus: {blurMat.GetFloat("_Radius")}");        
    }
}
