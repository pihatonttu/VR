
using UnityEngine;
/// <summary>
/// Valon virtanapin animointi
/// Tän vois tehä samassa kun valon säätö
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

}

