using UnityEngine;
using System.Collections;

public class HUDAnimationManager : MonoBehaviour 
{
	GameObject player;
	Animator anim;

	void Start () 
	{
		player = GameObject.FindGameObjectWithTag("Player");
		anim = GetComponent<Animator>();
	}
	
	void Update () 
	{
	
	}

	public void GameOver()
	{
		anim.SetTrigger("Death");
	}

	public void Pause()
	{
		anim.SetTrigger("Pause");
		Time.timeScale = 0f;
	}

	public void Resume()
	{
		Time.timeScale = 1f;
		anim.SetTrigger("Resume");
	}

	public void Victory()
	{
		GameObject.Destroy(player);
		anim.SetTrigger("Victory");
	}

	public void FinalScene()
	{
		anim = GameObject.FindGameObjectWithTag("FinalAnim").GetComponent<Animator>();
		anim.SetTrigger("Final");
	}
}
