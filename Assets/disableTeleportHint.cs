using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;


/// <summary>
/// Poistaa ohjeet teleporttauksesta
/// </summary>
public class disableTeleportHint : MonoBehaviour
{

	void Update()
	{
		Teleport.instance.CancelTeleportHint();
	}
}