using UnityEngine;
using System.Collections;

public class physicsCrateBehaviour : MonoBehaviour 
{
	public float deathFallSpeed;

	bool dying;
	
	void Update () 
	{
		if(rigidbody.velocity.y < deathFallSpeed && !dying)
			StartCoroutine(Death ());
	}

	IEnumerator Death()
	{
		dying = true;
		float deathTimeStamp = Time.time + 1f;
		if(!particleSystem.isPlaying)
			particleSystem.Play();
		while(deathTimeStamp > Time.time)
		{
			transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.1f,0.1f,0.1f), 2f * Time.deltaTime);
			yield return new WaitForEndOfFrame(); 
		}
		GameObject.Destroy(gameObject);
	}
}
