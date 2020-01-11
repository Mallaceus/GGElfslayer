using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
	[SerializeField] GameObject hammer;
	[SerializeField] GameObject hammerHead;
	[SerializeField] GameObject player;

	[SerializeField] float force = 50;
	[SerializeField] float hammerSmoothingSpeed = 10;
	[SerializeField] float hammerSmoothingDecreaseMulitplier = 0.05f;
	bool hammerInGround;

	Camera myCamera;
	Vector2 mousePos;
	Vector2 mouseVec;


	void Start()
    {
		myCamera = FindObjectOfType<Camera>();
    }

    void Update()
    {
		mousePos = myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));
		Vector2 centerRotVec = new Vector2(player.transform.position.x, player.transform.position.y);
		mouseVec = mousePos - centerRotVec;

		RotateHammer();
		//MoveHammer();
	}

	void MoveHammer()
	{
		hammer.transform.localPosition = Vector3.ClampMagnitude(mouseVec - mouseVec * 0.5f, 10);
	}

	void RotateHammer()
	{
		float angle = Mathf.Atan2(mouseVec.y, mouseVec.x) * Mathf.Rad2Deg;
		hammer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(hammer.transform.rotation.eulerAngles.z, angle, Time.deltaTime * hammerSmoothingSpeed * (hammerInGround ? 0.2f : 1))));

		//Vector2 forceVec = mousePos - new Vector2(hammerHead.transform.position.x, hammerHead.transform.position.y);
		//GetComponent<Rigidbody2D>().AddTorque(Vector3.Project(forceVec, hammerHead.transform.right).magnitude);
	}

	// When hammer hits an collider.
	private void OnCollisionStay2D(Collision2D collision)
	{
		hammerInGround = true;
		Vector2 forceVec = mousePos - new Vector2(hammerHead.transform.position.x, hammerHead.transform.position.y);
		player.GetComponent<Rigidbody2D>().AddForce(-hammerHead.transform.up * force * Mathf.Sign(collision.contacts[0].point.x - hammerHead.transform.position.x) * Mathf.Abs((mousePos - new Vector2(hammerHead.transform.position.x, hammerHead.transform.position.y)).x));
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		hammerInGround = false;

	}
}
