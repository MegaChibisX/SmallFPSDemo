using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon3_Missile : Attack
{

    public Rigidbody visualShot;
    public GameObject explosionPrefab;
    public LineRenderer line;
    protected GameObject ray;

    protected Vector3 startPos;
    protected RaycastHit hit;


    protected float movementCooldown = 60.0f;
    protected float speed = 000.0f;



    protected void Start()
    {
        startPos = transform.position;
    }
    protected void Update()
    {

        movementCooldown -= Time.deltaTime;

        if (movementCooldown <= 0.0f)
        {
            float maxDistance = float.PositiveInfinity;

            foreach (RaycastHit col in Physics.RaycastAll(transform.position, transform.forward))
            {
                if (maxDistance > col.distance)
                    maxDistance = col.distance;
            }
            if (float.IsPositiveInfinity(maxDistance))
            {
                line.transform.localScale = Vector3.one * 1000.0f;
            }
            else
            {
                line.transform.localScale = Vector3.one * maxDistance;
            }
        }
    }
    protected void FixedUpdate()
    {
        Movement();
        GetComponent<DestroyAfterTime>().time = 3.0f;
    }


    public void Launch()
    {
        movementCooldown = 0.0f;
    }

    protected void Movement()
    {
        if (movementCooldown <= 0.0f)
        {
            speed += Time.fixedDeltaTime * 2000.0f;

            Vector3 newPos = visualShot.transform.position + transform.forward * speed * Time.fixedDeltaTime;
            foreach (RaycastHit col in Physics.RaycastAll(visualShot.transform.position, visualShot.transform.forward, speed * Time.fixedDeltaTime))
            {
                // Damage if needed
                if (col.rigidbody != null)
                {
                    col.rigidbody.AddForceAtPosition(transform.forward * 50000.0f, col.point);
                    owner.minorSound.PlaySound(owner.wpnGui.hitSound, 1.0f, 0.0f);
                }
                else
                {
                    explosionPrefab = Instantiate(explosionPrefab);
                    explosionPrefab.transform.position = col.point;
                    explosionPrefab.transform.rotation = Quaternion.LookRotation(col.normal);
                    explosionPrefab.transform.localScale = Vector3.Scale(explosionPrefab.transform.localScale, transform.localScale);
                    Destroy(gameObject);
                }
            }

            visualShot.MovePosition(newPos);
            line.transform.position = startPos;
        }
    }

}
