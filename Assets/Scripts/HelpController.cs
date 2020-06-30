using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private GameObject fineControl;//Hienosäätö
    [SerializeField]
    private RevolverControl revolver;//Objektiivirevolveri
    [SerializeField]
    private GameObject lightSwitch;//Valokatkaisin
    [SerializeField]
    private GameObject lightControl;//Kirkkauden säätö rulla 
    [SerializeField]
    private GameObject slideHolder;//Näytteenpidin
    [SerializeField]
    private GameObject[] gameObjects;//
    [SerializeField]
    private GameObject[] slides;//Näytteet
    [SerializeField]
    private GameObject[] centralizeControls; //Keskiöintiruuvit
    [SerializeField]
    private GameObject[] axisNobs;
    

    private GameObject currentObj;
    private int currentIndex = 0;
    private bool HelpFinished = false;

    [SerializeField]
    bool Test = false;


    // Start is called before the first frame update
    void Start()
    {
        currentObj = gameObjects[currentIndex];
        NextInstruction();       
    }

    void Update()
    {
        //Testailua varten pikanappi
        if (Test)
        {
            NextInstruction();
            Test = false;        
        }
    }


    /// <summary>
    /// Ohjeiden eri vaiheet
    /// </summary>
    public void NextInstruction()
    {
        Debug.Log(currentIndex);
        if (HelpFinished)
        {
            return;
        }
        switch (currentIndex)
        {
            //Valo päälle
            case 0:
                ChangeTexts(RIGHT, "LIGHT_SWITCH");
                HighlightItem(lightSwitch);
                break;
            //Aseta näyte paikalleen
            case 1:
                UnHighlightLast(currentIndex);
                HighlightItem(slideHolder);
                HighlightItems(slides);
                ChangeTexts(RIGHT, "SLIDE_HOLDER");                
                break;
            //Säädä okulaarit silmille sopiviksi
            case 2:
                UnHighlightLast(currentIndex);
                UnHighlightOne(slideHolder);
                UnHighlightItems(slides);
                ChangeTexts(RIGHT, "OCULARS");           
                break;
            //Kirkkaudensäätö sopivaksi
            case 3:
                UnHighlightLast(currentIndex);
                ChangeTexts(RIGHT, "LIGHT_CONTROL");           
                break;
            //Valitse 10x objektiivi aluksi
            case 4:
                UnHighlightLast(currentIndex);                
                HighlightItems(axisNobs);
                ChangeTexts(RIGHT, "OBJECTIVE_REVOLVER");
                ChangeTexts(LEFT, "MECHANICAL_STAGE");
                break;
            //Kenttähimmentimen sulku puoleen väliin
            case 5:
                UnHighlightLast(currentIndex);                
                UnHighlightItems(axisNobs);
                ChangeTexts(RIGHT, "FIELD_DIAPHRAGM");
                ChangeTexts(LEFT, EMPTY);
                break;
            //Kondensorin säätö kunnes reunat terävät
            case 6:
                UnHighlightLast(currentIndex);
                ChangeTexts(RIGHT, "CONDENSER");
                ChangeTexts(LEFT, EMPTY);
                break;
            //Keskiöintiruuveilla kuva keskelle
            case 7:
                UnHighlightLast(currentIndex);
                HighlightItems(centralizeControls);
                ChangeTexts(RIGHT, "CENTRALIZING_CONTROLS");
                ChangeTexts(LEFT, EMPTY);
                break;
            //Kenttähimmennin auki kunnes reunat häviää kuvan ulkopuolelle
            case 8:
                UnHighlightLast(currentIndex);
                UnHighlightItems(centralizeControls);
                ChangeTexts(RIGHT, "FIELD_DIAPHRAGM", 2);
                ChangeTexts(LEFT, EMPTY);
                break;
            //Kuvan tarkentaminen
            case 9:
                UnHighlightLast(currentIndex);
                HighlightItem(fineControl);
                ChangeTexts(RIGHT, "COARSE_FOCUS");
                ChangeTexts(LEFT, "FINE_FOCUS");
                break;
            //Vaihda objektiivia tarkemmaksi
            case 10:
                UnHighlightLast(currentIndex);
                ChangeTexts(RIGHT, "OBJECTIVE_REVOLVER", 2);
                ChangeTexts(LEFT, EMPTY);
                break;
            //Valmis
            case 11:
                UnHighlightLast(currentIndex);
                ChangeTexts(RIGHT, "DONE");
                ChangeTexts(LEFT, "DONE");
                currentIndex = 0;
                HelpFinished = true;
                break;
            
            default:
                break;
        }
        
        if (currentIndex < gameObjects.Length - 1)
        {
            if (currentObj)
            {
                HighlightItem(currentObj);
            }            
            currentIndex++;
            currentObj = gameObjects[currentIndex];
        }
        
        
    }

    /// <summary>
    /// Vaihdetaan teksti
    /// </summary>
    /// <param name="side">Kummalle puolelle</param>
    /// <param name="_param">Parametri</param>
    /// <param name="helpTextNumber">Mikä ohje menossa</param>
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

    //Poista korostus viimeisimmästä
    private void UnHighlightLast(int index)
    {
        gameObjects[index - 1].GetComponent<Outline>().OutlineWidth = ZERO;
        gameObjects[index - 1].GetComponent<Outline>().OutlineColor = defaultColor;
    }

    //Poista korostus yhdestä
    private void UnHighlightOne(GameObject item)
    {
        item.GetComponent<Outline>().OutlineWidth = ZERO;
        item.GetComponent<Outline>().OutlineColor = defaultColor;
    }

    //Poista korostus listan esineistä
    private void UnHighlightItems(GameObject[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<Outline>().OutlineWidth = ZERO;
            items[i].GetComponent<Outline>().OutlineColor = defaultColor;
        }
    }

    //Korosta esineet listasta
    private void HighlightItems(GameObject[] items)
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<Outline>().OutlineColor = highlightColor;
            items[i].GetComponent<Outline>().OutlineWidth = highlightWidth;            
        }
    }

    //Korosta yksittäinen esine
    private void HighlightItem(GameObject item)
    {
        item.GetComponent<Outline>().OutlineColor = highlightColor;
        item.GetComponent<Outline>().OutlineWidth = highlightWidth;
    }

    //Nollaa ohjeet
    public void ResetHelp()
    {
        if (!HelpFinished)
        {
            ChangeTexts(RIGHT, EMPTY);
            ChangeTexts(LEFT, EMPTY);
            currentObj = gameObjects[0];
            NextInstruction();
            Debug.Log("Reset Triggered");
        }
    }   

    //Hae nykyinen ohje objekti
    public GameObject GetCurrentHelpObject()
    {
        return gameObjects[currentIndex -1];
    }
}
