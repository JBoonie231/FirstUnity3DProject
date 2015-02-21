using UnityEngine;
using System.Collections;

public class fenceBehaviour : MonoBehaviour 
{
	public bool isOneTimePowerOff;
	bool fenceOn = true;

	void Update () 
	{
		if((isOneTimePowerOff && fenceOn) || !isOneTimePowerOff)
		{
			fenceOn = false;

			foreach(switchBehaviour switchPower in GetComponentsInChildren<switchBehaviour>())
			{
				if(!switchPower.on)
				{
					fenceOn = true;
					break;
				}
			}
			foreach(targetBehaviour targetPower in GetComponentsInChildren<targetBehaviour>())
			{
				if(!targetPower.on)
				{
					fenceOn = true;
					break;
				}
			}

			renderer.enabled = fenceOn;
			collider.enabled = fenceOn;
		}
	}
}
