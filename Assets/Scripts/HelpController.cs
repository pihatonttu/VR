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
    //0. Valonappi
    [SerializeField]
    private GameObject lightSwitch;

    //1. Näytteet
    [SerializeField]
    private GameObject[] slides;

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

    //VANHOJA OHJEJUTTUJA

    //[SerializeField]
    //private GameObject slideHolder;//Näytteenpidin

    //[SerializeField]
    //private GameObject[] gameObjects;

    //private GameObject currentObj;


    private int currentIndex = -1;
    private bool HelpFinished = false;

    public List<GameObject> HiglightedItems;

    [SerializeField]
    bool Test = false;


    // Start is called before the first frame update
    void Start()
    {
        //currentObj = gameObjects[currentIndex];
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
            //Valo päälle
            case 0:       
                HighlightItem(lightSwitch);
                ChangeTexts(RIGHT, "LIGHT_SWITCH");
                break;
            //Aseta näyte paikalleen
            case 1:
                //HighlightItem(slideHolder);
                HighlightItems(slides);
                ChangeTexts(RIGHT, "SLIDE_HOLDER");                
                break;
            //Säädä okulaarit silmille sopiviksi
            case 2:
                HighlightItems(oculars);
                ChangeTexts(RIGHT, "OCULARS");           
                break;
            //Kirkkaudensäätö sopivaksi
            case 3:
                HighlightItem(lightControl);
                ChangeTexts(RIGHT, "LIGHT_CONTROL");           
                break;
            //Valitse 10x objektiivi aluksi
            case 4:              
                HighlightItem(revolver);
                ChangeTexts(RIGHT, "OBJECTIVE_REVOLVER");
                ChangeTexts(LEFT, "MECHANICAL_STAGE");
                break;
            //Kenttähimmentimen sulku puoleen väliin
            case 5:
                HighlightItem(fieldDiaphragm);
                ChangeTexts(RIGHT, "FIELD_DIAPHRAGM");
                ChangeTexts(LEFT, EMPTY);
                break;
            //Kondensorin säätö kunnes reunat terävät
            case 6:
                HighlightItem(CONDENSER);
                ChangeTexts(RIGHT, "CONDENSER");
                ChangeTexts(LEFT, EMPTY);
                break;
            //Keskiöintiruuveilla kuva keskelle
            case 7:
                HighlightItems(centralizeControls);
                ChangeTexts(RIGHT, "CENTRALIZING_CONTROLS");
                ChangeTexts(LEFT, EMPTY);
                break;
            //Kenttähimmennin auki kunnes reunat häviää kuvan ulkopuolelle
            case 8:
                HighlightItem(fieldDiaphragm);
                ChangeTexts(RIGHT, "FIELD_DIAPHRAGM", 2);
                ChangeTexts(LEFT, EMPTY);
                break;
            //Kuvan tarkentaminen
            case 9:
                HighlightItem(fineControl);
                HighlightItem(coarseControl);
                ChangeTexts(RIGHT, "COARSE_FOCUS");
                ChangeTexts(LEFT, "FINE_FOCUS");
                break;
            //Vaihda objektiivia tarkemmaksi
            case 10:
                HighlightItem(revolver);
                ChangeTexts(RIGHT, "OBJECTIVE_REVOLVER", 2);
                ChangeTexts(LEFT, EMPTY);
                break;
            //Valmis
            case 11:
                ChangeTexts(RIGHT, "DONE");
                ChangeTexts(LEFT, "DONE");
                currentIndex = 0;
                HelpFinished = true;
                break;        
            default:
                break;
        }
        
        //if (currentIndex < gameObjects.Length - 1)
        //{       
        //    currentIndex++;
        //    currentObj = gameObjects[currentIndex];
        //}
        
        
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
            else
            {
                leftHeading.ChangeParam(EMPTY);
                leftHelp.ChangeParam(EMPTY);
                leftInfo.ChangeParam(EMPTY);
            }
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
            rightHeading.ChangeParam(Param);            
            rightInfo.ChangeParam(Param + "_INFO");            
            rightHelp.ChangeParam(Param + helpParam);
        }
        else if (side == LEFT)
        {
            leftHeading.ChangeParam(Param);
            leftHelp.ChangeParam(Param + helpParam);
            leftInfo.ChangeParam(Param + "_INFO");
        }
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
            ChangeTexts(LEFT, EMPTY);
            //currentObj = gameObjects[0];
            NextInstruction();
            Debug.Log("Reset Triggered");
        }
    }   
}
