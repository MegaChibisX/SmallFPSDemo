using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon4_Fan : Attack
{

    public Weapon4 ownerWpn;

    public Rigidbody body;
    public Transform mesh;

    public ParticleSystem trail;
    public ParticleSystem glow;
    public ParticleSystem trailFull;


    [System.NonSerialized]
    public float speed = 3.0f;
    [System.NonSerialized]
    public float charge = 0.0f;



    protected void Start()
    {
        mesh.transform.rotation = Quaternion.AngleAxis(Random.Range(0,30), transform.forward) * mesh.transform.rotation;
        if (charge > 0.2f)
            trail.Play();
        if (charge > 2.5f)
            glow.Play();
        if (charge > 4.5f)
            trailFull.Play();
    }

    protected void Update()
    {
        if (speed > -3.0f)
        {
            speed -= Time.deltaTime;
            mesh.transform.rotation = Quaternion.AngleAxis(-10.0f * Time.deltaTime * speed, transform.forward) * mesh.transform.rotation;
        }
        if (speed < 0.0f && Vector3.Distance(transform.position, owner.transform.position) < 2.0f)
        {
            Destroy(gameObject);
        }

        mesh.transform.rotation = Quaternion.AngleAxis(720.0f * Time.deltaTime, mesh.transform.up) * mesh.transform.rotation;

    }

    protected void FixedUpdate()
    {
        if (speed > 0.0f)
            body.MovePosition(transform.position + transform.forward * Mathf.Clamp(speed, 1, 100) * (10.0f + charge) * Time.fixedDeltaTime);
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(owner.transform.position - transform.position),
                                                                              270.0f * Mathf.Clamp(-speed, 1, 2) * Time.fixedDeltaTime);
            body.MovePosition(transform.position + transform.forward * (30.0f + charge) * Time.fixedDeltaTime);
        }
    }


    public void OnDestroy()
    {
        ownerWpn.fansAvailable++;
    }

    private void OnTriggerEnter(Collider other)
    {

        Rigidbody otherBody;
        if ((otherBody = other.attachedRigidbody) != null && otherBody.transform != owner.transform)
        {
            otherBody.AddForceAtPosition(transform.forward * 50000.0f, transform.position);
            otherBody.AddForce(Vector3.up * (2.0f + charge * 2.0f), ForceMode.VelocityChange);
            float distance01 = Mathf.Clamp(100.0f - Vector3.Distance(owner.transform.position, otherBody.transform.position) * 0.5f, 50.0f, 100.0f) * 0.01f;
            owner.minorSound.PlaySound(owner.wpnGui.hitSound, distance01, 0.0f);
        }
    }


}
