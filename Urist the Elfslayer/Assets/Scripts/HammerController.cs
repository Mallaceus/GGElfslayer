using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController : MonoBehaviour
{
	[SerializeField] GameObject hammer;
	[SerializeField] GameObject hammerHead;
	[SerializeField] GameObject player;
	Rigidbody2D playerRigidBody;

	[SerializeField] float playerForce = 50;
	[SerializeField] float hammerSmoothingSpeed = 10;
	[SerializeField] float hammerSmoothingDecreaseMulitplier = 0.05f;
	bool hammerInGround;
	[SerializeField] float minHammerSpeedForDamage = 5;
	Camera myCamera;
	Vector2 mousePos;
	Vector2 mouseVec;


	void Start()
    {
		myCamera = FindObjectOfType<Camera>();
		playerRigidBody = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
		mousePos = myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -myCamera.transform.position.z));
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
	}

	// When hammer hits an collider.
	private void OnCollisionStay2D(Collision2D collision)
	{
		float hammerSpeed = Mathf.Exp(Mathf.Abs((mousePos - new Vector2(hammerHead.transform.position.x, hammerHead.transform.position.y)).x) * 0.7f);

		var destructible = collision.gameObject.GetComponent<Destructible>();
		if (destructible && hammerSpeed > minHammerSpeedForDamage)
		{
			destructible.Hurt(5000, 0, collision.contacts[0].point);
		}

		// Move player according to the distance between the hammerhead and mousePos.
		else
		{
			hammerInGround = true;
			Vector2 forceVec = mousePos - new Vector2(hammerHead.transform.position.x, hammerHead.transform.position.y);
			playerRigidBody.AddForce(-hammerHead.transform.up * playerForce * Mathf.Sign(collision.contacts[0].point.x - hammerHead.transform.position.x) * hammerSpeed);
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		hammerInGround = false;
	}
}
