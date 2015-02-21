using UnityEngine;
using System.Collections;

public class finalSceneCameraController : MonoBehaviour 
{
	GameObject spaceShip;

	void Start () 
	{
		spaceShip = GameObject.FindGameObjectWithTag("SpaceShip");
	}
	
	void Update () 
	{
		transform.LookAt(spaceShip.transform);
	}
}
