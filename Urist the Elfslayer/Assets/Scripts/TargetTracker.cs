using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTracker : MonoBehaviour
{
	[SerializeField] Transform target;
	[SerializeField] float speed = 100;
	[SerializeField] float deadzone = 0;

    void Update()
    {
		if(Vector2.Distance(transform.position, target.position) > deadzone)
		{
			Vector2 lerp = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), new Vector2(target.position.x, target.position.y), Time.deltaTime * speed);
			transform.position = new Vector3(lerp.x, lerp.y, transform.position.z);
		}
    }
}
