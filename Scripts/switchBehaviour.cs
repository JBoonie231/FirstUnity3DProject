using UnityEngine;
using System.Collections;

public class switchBehaviour : MonoBehaviour 
{
	public bool isOneTimeSwitch;
	public bool on;

	void OnCollisionStay(Collision collision)
	{
		on = true;

		renderer.material.SetColor("_Color", Color.green); 

		foreach(Renderer powerLine in GetComponentsInChildren<Renderer>())
		{
			powerLine.material.SetColor("_Color", Color.green);
		}
	}

	void OnCollisionExit(Collision collision)
	{
		if(!isOneTimeSwitch)
		{
			on = false;

			renderer.material.SetColor("_Color", Color.red); 

			foreach(Renderer powerLine in GetComponentsInChildren<Renderer>())
			{
				powerLine.material.SetColor("_Color", Color.red);
			}
		}
	}
}
