
using UnityEngine;
/// <summary>
/// Valon virtanappi
/// </summary>
public class LightToggler : MonoBehaviour
{

    [SerializeField]
    private GameObject lightSwitch;
    [SerializeField]
    private LightControl lightControl;
    [SerializeField]
    private Animator animator;
    private bool lightOn = false;

    private HelpController Tutorial;

    private void Awake()
    {
        Tutorial = FindObjectOfType<HelpController>();   
    }

    public void ToggleLight()
    {
        if (lightOn)
        {
            animator.Play("SwitchOff");
        }
        else
        {
            animator.Play("SwitchOn");
        }

        lightOn = !lightOn;
        lightControl.ToggleLight(lightOn);

    }
    public bool IsLightOn()
    {
        return lightOn;
    }

}

