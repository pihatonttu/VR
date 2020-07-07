using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Sample : MonoBehaviour
{
    //ScriptableObjekti
    public SampleObject SampleScriptableObject;

    //Näyte GameObjekti
    public GameObject SampleTexture;

    //Paikka jossa nimi näytetään (haetaan automaattisesti)
    TextMesh SampleNameText;

    public Texture Mag60 { get => SampleScriptableObject.TextureOf60Mag; set => SampleScriptableObject.TextureOf60Mag = value; }
    public Texture Mag20 { get => SampleScriptableObject.TextureOf20Mag; set => SampleScriptableObject.TextureOf20Mag = value; }
    public Texture Mag10 { get => SampleScriptableObject.TextureOf10Mag; set => SampleScriptableObject.TextureOf10Mag = value; }
    public Vector2 OffsetStart { get => SampleScriptableObject.OffsetStart; set => SampleScriptableObject.OffsetStart = value; }

    private void Awake()
    {
        //Laitetaan teksti valmiiksi teksti objektiin ja piilotetaan jos ei ole sitä jo tehty
        if (GetComponentInChildren<TextMesh>())
        {
            SampleNameText = GetComponentInChildren<TextMesh>();
            SampleNameText.text = SampleScriptableObject.SampleName;

            if (SampleNameText.gameObject.activeSelf)
                SampleNameText.gameObject.SetActive(false);
        }
        else
            Debug.LogError("Näytteestä " + SampleScriptableObject.SampleName + " ei löytynyt textMesh komponenttia!");

        //Laitetaan tekstuuri objektiin näkyviin
        if (SampleTexture != null)
        {
            SampleTexture.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", SampleScriptableObject.GameWorldTexture);
        }
        else
            Debug.LogError("Näytteestä " + SampleScriptableObject.SampleName + " Näytteelle paikkaa!");

    }

    //Kun käsi laitetaan triggeri alueelle
    private void OnHandHoverBegin(Hand hand)
    {
        hand.ShowGrabHint();
        SampleNameText.gameObject.SetActive(true);      
    }

    //Kun käsi poistuu triggeri alueelta
    private void OnHandHoverEnd(Hand hand)
    {
        hand.HideGrabHint();
        SampleNameText.gameObject.SetActive(false);
    }
}
