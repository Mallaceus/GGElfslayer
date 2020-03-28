using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerController2 : MonoBehaviour
{

    [SerializeField] GameObject hammerHead;
    [SerializeField] GameObject player;
    Rigidbody2D playerRigidBody;

    [SerializeField] float playerForce = 50;
    [SerializeField] float playerMaxVelocity = 10f;
    [SerializeField] float hammerSmoothingSpeed = 10;
    [SerializeField] float hammerStrength = 100;
    [SerializeField] float minHammerSpeedForDamage = 5;
    [SerializeField] float damage = 10;
    [SerializeField] float hammerMass = 10;

    Vector2 hammerheadVelocity;
    Vector2 lastHammerheadPos;

    Camera myCamera;
    Vector2 mousePos;
    Vector2 mouseVec;

    bool hammerInGround;

    // Start is called before the first frame update
    void Start()
    {
        myCamera = FindObjectOfType<Camera>();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mousePos = myCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -myCamera.transform.position.z));
        Vector2 centerRotVec = new Vector2(player.transform.position.x, player.transform.position.y);
        mouseVec = mousePos - centerRotVec;

        CalculateVelocity();

        RotateHammer();
    }

    void HammerHold()
    {
        
    }

    void CalculateVelocity()
    {
        hammerheadVelocity = new Vector2(hammerHead.transform.position.x - lastHammerheadPos.x, hammerHead.transform.position.y - lastHammerheadPos.y);
        lastHammerheadPos = hammerHead.transform.position;
    }

    void RotateHammer()
    {
        float angle = Mathf.Atan2(mouseVec.y, mouseVec.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.LerpAngle(transform.rotation.eulerAngles.z, angle, Time.deltaTime * hammerSmoothingSpeed * (hammerInGround ? 0.2f : 1))));
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        var destructible = collision.gameObject.GetComponent<Destructible>();
        if (destructible && hammerheadVelocity.magnitude > minHammerSpeedForDamage)
        {
            destructible.Hurt(damage, 0, collision.contacts[0].point);
        }

        // Move player according to the distance between the hammerhead and mousePos.
        else
        {
            hammerInGround = true;
            playerRigidBody.velocity -= (hammerheadVelocity);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        hammerInGround = false;
    }
}
