using UnityEngine;
using System.Collections;

public class spawnerBehaviour : MonoBehaviour 
{
	public GameObject spawnElement;
	public bool unlimited;
	public bool atLeastOne;
	public bool startLeft;

	GameObject currentSpawn;
	
	void Update () 
	{
		if(atLeastOne)
			Spawn();

		if(unlimited || currentSpawn == null)
		{
			foreach(switchBehaviour switchPower in GetComponentsInChildren<switchBehaviour>())
			{
				if(switchPower.on)
				{
					Spawn ();
					break;
				}
			}
			foreach(targetBehaviour targetPower in GetComponentsInChildren<targetBehaviour>())
			{
				if(targetPower.on)
				{
					Spawn();
					break;
				}
			}
		}
	}

	public void Spawn()
	{
		if(unlimited || currentSpawn == null)
		{
			currentSpawn = (GameObject)Instantiate(spawnElement, transform.position, spawnElement.transform.rotation);

			if(startLeft)
			{
				float yRotation = currentSpawn.transform.eulerAngles.y;
				currentSpawn.transform.eulerAngles = new Vector3(0f, -yRotation, 0f);
			}
		}
	}
}
