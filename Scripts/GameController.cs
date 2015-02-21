using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour 
{
	public float spawnRate;

	HUDAnimationManager hudAnimationManager;
	GameObject[] spawners;
	float spawnCooldown;

	void Start () 
	{
		hudAnimationManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDAnimationManager>();
		spawners = GameObject.FindGameObjectsWithTag("Spawner");
	}
	
	void Update () 
	{
		if(Input.GetKey(KeyCode.Escape))
			hudAnimationManager.Pause();

		if(spawnCooldown < Time.time)
		{
			spawnCooldown = spawnRate + Time.time;
			foreach(GameObject spawner in spawners)
			{
				if(spawner.GetComponent<spawnerBehaviour>().unlimited)
					spawner.GetComponent<spawnerBehaviour>().Spawn();
			}
		}
	}

	public void Restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}

	public void NextLevel()
	{
		int i = Application.loadedLevel;
		Application.LoadLevel(i + 1);
	}

	public void Quit()
	{
		Application.Quit();
	}
}
