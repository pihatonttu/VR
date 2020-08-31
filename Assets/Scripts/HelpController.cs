using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Valve.VR.InteractionSystem;

/// <summary>
/// Ohjeet ensimmäiselle pelikerralle
/// </summary>
public class HelpController : MonoBehaviour
{
    [SerializeField]
    private Color highlightColor;
    [SerializeField]
    private Color defaultColor;

    private const float ZERO = 0;
    private const string RIGHT = "Right";
    private const string LEFT = "Left";
    private const string EMPTY = "EMPTY";

    [SerializeField]
    private float highlightWidth;

    //Ohjepaikat
    [SerializeField]
    private TranslatedText rightHeading;
    [SerializeField]
    private TranslatedText rightHelp;
    [SerializeField]
    private TranslatedText rightInfo;
    [SerializeField]
    private TranslatedText leftHeading;
    [SerializeField]
    private TranslatedText leftHelp;
    [SerializeField]
    private TranslatedText leftInfo;
    //---------------------------------------

    //Korostettavat kohteet
    //-1. Huppu
    [SerializeField]
    private GameObject hood;

    //0. Valonappi
    [SerializeField]
    private GameObject lightSwitch;

    //1. Näytteet
    [SerializeField]
    private GameObject[] slides;
    [SerializeField]
    private SamplePlaceTrigger samplePlace;

    //2. Okulaarit
    [SerializeField]
    private GameObject[] oculars;

    //3. Valonsäätö
    [SerializeField]
    private GameObject lightControl;

    //4. ja 10. Objektiivi revolveri
    [SerializeField]
    private GameObject revolver;

    //5. ja 8. Kenttähimmennin
    [SerializeField]
    private GameObject fieldDiaphragm;

    //6. Kondensori 
    [SerializeField]
    private GameObject CONDENSER;

    //7. Keskiöintiruuvit
    [SerializeField]
    private GameObject[] centralizeControls;

    //9. Hieno ja karkea säätö
    [SerializeField]
    private GameObject fineControl;
    [SerializeField]
    private GameObject coarseControl;

    //Kuvan liikuttelu 
    //HUOM! ei vielä ohjetta tälle
    [SerializeField]
    private GameObject[] axisNobs;
    //------------------------------------------------------

    public int currentIndex = -1;
    private bool HelpFinished = false;

    public List<GameObject> HiglightedItems;

    [SerializeField]
    bool Test = false;

    [SerializeField]
    bool tutorial = true;
    [SerializeField]
    FocusControl fc;


    // Start is called before the first frame update
    void Start()
    {
        //currentObj = gameObjects[currentIndex];
        NextInstruction();       
    }

    private void Update()
    {
        if (tutorial && currentIndex < 15)
        {
            float tmp = 0;

            //Kaikki ohjevaiheet
            //TODO: Tähän vois keksiä paremman keinon
            if (hood.GetComponent<CheckForHood>().IsHoodOn == false && currentIndex == 0)
                NextInstruction();
            if (lightSwitch.GetComponent<LightToggler>().IsLightOn() && currentIndex == 1)
                NextInstruction();
            if (lightControl.GetComponent<CircularDrive>().outAngle > 100 && currentIndex == 2)
                NextInstruction();
            if (samplePlace.IsInPlace() && currentIndex == 3)
                NextInstruction();
            if ((axisNobs[0].GetComponent<CircularDrive>().outAngle != 0 || axisNobs[1].GetComponent<CircularDrive>().outAngle != 0) && currentIndex == 4)
                NextInstruction();
            if ((oculars[0].GetComponent<CircularDrive>().outAngle != 0 || oculars[1].GetComponent<CircularDrive>().outAngle != 360) && currentIndex == 5)
                NextInstruction();
            if (lightControl.GetComponent<CircularDrive>().outAngle > 200 && currentIndex == 6)
                NextInstruction();
            if (CONDENSER.GetComponent<CircularDrive>().outAngle != 0 && currentIndex == 7)
                NextInstruction();
            if (revolver.GetComponent<RevolverControl>().GetCurrentMagnification() == 10 && currentIndex == 8)
                NextInstruction();
            if (fieldDiaphragm.GetComponent<CircularDrive>().outAngle < 70 && currentIndex == 9)
            {
                NextInstruction();
                tmp = CONDENSER.GetComponent<CircularDrive>().outAngle;
            }
            if (CONDENSER.GetComponent<CircularDrive>().outAngle != tmp && currentIndex == 10)
                NextInstruction();
            if ((centralizeControls[0].GetComponent<CircularDrive>().outAngle != 0 || centralizeControls[1].GetComponent<CircularDrive>().outAngle != 0) && currentIndex == 11)
                NextInstruction();
            if (fieldDiaphragm.GetComponent<CircularDrive>().outAngle > 150 && currentIndex == 12)
                NextInstruction();
            if (fc.GetTableHeightComparedToTarget() < 0.1f && currentIndex == 13)
                NextInstruction();
            if (revolver.GetComponent<RevolverControl>().GetCurrentMagnification() != 10 && revolver.GetComponent<RevolverControl>().GetCurrentMagnification() != 0 && currentIndex == 14)
                NextInstruction();
            if (currentIndex == 15)
                NextInstruction();
        }
    }

    //Mennään ohjeessa taaksepäin
    public void PreviousInstruction()
    {
        //Otetaan kaksi taaksepäin koska nextinstruction lisää yhden alussa
        currentIndex -= 2;
        NextInstruction();
    }

    /// <summary>
    /// Ohjeiden eri vaiheet
    /// </summary>
    public void NextInstruction()
    {
        currentIndex++;
        //Debug.Log(currentIndex);
        UnHighlightAll();

        if (HelpFinished)
            return;

        switch (currentIndex)
        {
            case 0:
                HighlightItem(hood);
                ChangeTexts(RIGHT, "HOOD");
                break;
            //Valo päälle
            case 1:       
                HighlightItem(lightSwitch);
                ChangeTexts(RIGHT, "LIGHT_SWITCH");
                break;          
            //Kirkkaudensäätö sopivaksi
            case 2:
                HighlightItem(lightControl);
                ChangeTexts(RIGHT, "LIGHT_CONTROL");
                break;
            //Aseta näyte paikalleen
            case 3:
                HighlightItems(slides);
                ChangeTexts(RIGHT, "SLIDE_HOLDER");
                break;     
            //Ristisiirtopöydän siirto
            case 4:
                HighlightItems(axisNobs);
                ChangeTexts(RIGHT, "MECHANICAL_STAGE");
                break;       
            //Säädä okulaarit silmille sopiviksi
            case 5:
                HighlightItems(oculars);
                ChangeTexts(RIGHT, "OCULARS");
                break;     
            //Valonsäätö tarvittaessa
            case 6:
                HighlightItem(lightControl);
                ChangeTexts(RIGHT, "LIGHT_CONTROL2");
                break;
            //Kondensorin korkeuden säätö ylös
            case 7:
                HighlightItem(CONDENSER);
                ChangeTexts(RIGHT, "CONDENSER");
                break;          
            //Valitse 10x objektiivi
            case 8:
                HighlightItem(revolver);
                ChangeTexts(RIGHT, "OBJECTIVE_REVOLVER");
                break;
            //Kenttähimmentimen sulku vähintään puoleen väliin
            case 9:
                HighlightItem(fieldDiaphragm);
                ChangeTexts(RIGHT, "FIELD_DIAPHRAGM");
                break;    
            //Kondensorin säätö kunnes reunat terävät
            case 10:
                HighlightItem(CONDENSER);
                ChangeTexts(RIGHT, "CONDENSER2");
                break;
            //Keskiöintiruuveilla aukko keskelle
            case 11:
                HighlightItems(centralizeControls);
                ChangeTexts(RIGHT, "CENTRALIZING_CONTROLS");
                break;
            //Kenttähimmennin auki kunnes reunat häviää kuvan ulkopuolelle
            case 12:
                HighlightItem(fieldDiaphragm);
                ChangeTexts(RIGHT, "FIELD_DIAPHRAGM2");
                break;
            //Kuvan tarkentaminen
            case 13:
                HighlightItem(fineControl);
                HighlightItem(coarseControl);
                ChangeTexts(RIGHT, "COARSE_FOCUS");
                break;
            //Vaihda objektiivia
            case 14:
                HighlightItem(revolver);
                ChangeTexts(RIGHT, "OBJECTIVE_REVOLVER2");
                break;
            //Valmis
            case 15:
                ChangeTexts(RIGHT, "DONE");
                currentIndex = 0;
                HelpFinished = true;
                break;        
            default:
                break;
        }
        
        
        
    }

    public void NextInstructionButton()
    {
        NextInstruction();
    }

    //Vaihdetaan teksti
    private void ChangeTexts (string side, string _param, int helpTextNumber=1)
    {
        string Param = _param;
        string helpParam = "_HELP";

        //Jos parametrina tulee "EMPTY" niin tyhjennetään puolisko
        if (Param == EMPTY)
        {
            if (side == RIGHT)
            {
                rightHeading.ChangeParam(EMPTY);
                rightHelp.ChangeParam(EMPTY);
                rightInfo.ChangeParam(EMPTY);
            }
            //else
            //{
            //    leftHeading.ChangeParam(EMPTY);
            //    leftHelp.ChangeParam(EMPTY);
            //    leftInfo.ChangeParam(EMPTY);
            //}
            return;
        }
        //Jos parametrina ei tullut emptyä niin lisätään ohjeet "side":n puolelle
        else if (side == RIGHT)
        {
            if (helpTextNumber != 1)
            {
                helpParam = "_HELP" + helpTextNumber;
            }
            else
            {
                helpParam = "_HELP";
            }
            //rightHeading.ChangeParam(Param);            
            rightInfo.ChangeParam(Param + "_INFO");            
            rightHelp.ChangeParam(Param + helpParam);
        }
        //else if (side == LEFT)
        //{
        //    leftHeading.ChangeParam(Param);
        //    leftHelp.ChangeParam(Param + helpParam);
        //    leftInfo.ChangeParam(Param + "_INFO");
        //}
    }

    //Poista korostus kaikista nykyisistä korostetuista esineistä
    private void UnHighlightAll()
    {
        if (HiglightedItems != null)
        {
            foreach (GameObject item in HiglightedItems)
            {
                if (item.GetComponent<Highlight>())
                {
                    item.GetComponent<Highlight>().OutlineWidth = ZERO;
                    item.GetComponent<Highlight>().OutlineColor = defaultColor;
                    item.GetComponent<Highlight>().enabled = false;
                }
                else
                    Debug.LogError("Korostus komponenttia ei löytynyt " + item.name);
            }
            HiglightedItems.Clear();

            if (Test)
                Debug.Log("Korostus poistettu kaikista. HighlightLista: " + HiglightedItems.Count);
        }     
    }

    //Poista korostus yhdestä
    private void UnHighlightOne(GameObject item)
    {
        if (item.GetComponent<Highlight>())
        {
            item.GetComponent<Highlight>().OutlineWidth = ZERO;
            item.GetComponent<Highlight>().OutlineColor = defaultColor;
            HiglightedItems.Remove(item);
            item.GetComponent<Highlight>().enabled = false;
            item.GetComponent<Highlight>().CurrentlyOn = false;
        }
        else
            Debug.LogError("Korostus komponenttia ei löytynyt " + item.name);
    }

    //Poista korostus listan esineistä
    private void UnHighlightItems(GameObject[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].GetComponent<Highlight>())
            {
                items[i].GetComponent<Highlight>().OutlineWidth = ZERO;
                items[i].GetComponent<Highlight>().OutlineColor = defaultColor;
                items[i].GetComponent<Highlight>().enabled = false;
                items[i].GetComponent<Highlight>().CurrentlyOn = false;
            }
            else
                Debug.LogError("Korostus komponenttia ei löytynyt " + items[i].name);
        }
    }

    //Korosta esineet listasta jos niistä löytyy korostus komponentti
    private void HighlightItems(GameObject[] items)
    {
        foreach (GameObject item in items)
        {
            if (item.GetComponent<Highlight>())
            {
                item.GetComponent<Highlight>().enabled = true;
                item.GetComponent<Highlight>().OutlineColor = highlightColor;
                item.GetComponent<Highlight>().OutlineWidth = highlightWidth;
                item.GetComponent<Highlight>().CurrentlyOn = true;
                HiglightedItems.Add(item);
            } else
                Debug.LogError("Korostus komponenttia ei löytynyt " + item.name);
        }
    }

    //Korosta yksittäinen esine jos sillä on korostus komponentti
    private void HighlightItem(GameObject item)
    {
        if (item.GetComponent<Highlight>())
        {
            item.GetComponent<Highlight>().enabled = true;
            item.GetComponent<Highlight>().CurrentlyOn = true;
            item.GetComponent<Highlight>().OutlineColor = highlightColor;
            item.GetComponent<Highlight>().OutlineWidth = highlightWidth;
            HiglightedItems.Add(item);
            
            if (Test) 
                Debug.Log("Korostetaan: " + item.name);
        } else
        {
            Debug.LogError("Korostus komponenttia ei löytynyt: " + item.name);
        }
    }

    //Nollaa ohjeet
    public void ResetHelp()
    {
        if (!HelpFinished)
        {
            ChangeTexts(RIGHT, EMPTY);
            //ChangeTexts(LEFT, EMPTY);
            //currentObj = gameObjects[0];
            NextInstruction();
            Debug.Log("Reset Triggered");
        }
    }   
}
