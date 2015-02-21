using UnityEngine;
using System.Collections;

public class levelExitBehaviour : MonoBehaviour 
{
	public bool lastLevel;

	GameObject hud;
	GameObject player;

	void Start () 
	{
		hud = GameObject.FindGameObjectWithTag("HUD");
		player = GameObject.FindGameObjectWithTag("Player");
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == player)
		{
			if(lastLevel)
			{
				hud.GetComponent<HUDAnimationManager>().FinalScene();
			}
			else
				hud.GetComponent<HUDAnimationManager>().Victory();
		}
	}

}
