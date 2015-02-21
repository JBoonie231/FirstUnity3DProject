using UnityEngine;
using System.Collections;

public class targetBehaviour : MonoBehaviour 
{
	public bool on;
	public float timerCooldown;

	Light spotlight;
	float timerTimeStamp;
	// Use this for initialization
	void Start () 
	{
		spotlight = GetComponentInChildren<Light>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(timerTimeStamp < Time.time)
		{
			on = false;
			spotlight.color = Color.red;

			foreach(Renderer powerLine in GetComponentsInChildren<Renderer>())
			{
				if(powerLine.gameObject != gameObject)
					powerLine.material.SetColor("_Color", Color.red);
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.name == "FireBall" || other.name == "FireBall(Clone)" )
		{
			timerTimeStamp = timerCooldown + Time.time;
			on = true;
			spotlight.color = Color.green; 
			
			foreach(Renderer powerLine in GetComponentsInChildren<Renderer>())
			{
				if(powerLine.gameObject != gameObject)
					powerLine.material.SetColor("_Color", Color.green);
			}
		}
	}
}
