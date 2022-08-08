using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Explosion : Attack
{


    public float force = 10000.0f;
    public float diameter = 0.5f;

    protected bool disableOnNextFrame = false;


    private void Start()
    {
        transform.localScale = Vector3.one * diameter * 2.0f;
        StartCoroutine(fadeOut());
    }



    protected void ApplyForceAroundArea()
    {
        float distanceVolume = 0.0f;
        RaycastHit hit;
        foreach (Collider col in Physics.OverlapSphere(transform.position, diameter))
        {
            Vector3 dir = col.transform.position - transform.position;
            Ray ray = new Ray(transform.position, dir);
            if (col.Raycast(ray, out hit, dir.magnitude) && hit.rigidbody != null)
            {
                hit.rigidbody.AddExplosionForce(force / hit.rigidbody.mass, hit.point - ray.direction * 0.1f, diameter * 10.0f, 0.2f, ForceMode.VelocityChange);
                distanceVolume += 2.0f - (hit.distance / diameter * 4.0f);
            }
        }

        if (owner != null && distanceVolume >= owner.minorSound.volume)
            owner.minorSound.PlaySound(owner.wpnGui.hitSound, distanceVolume, 0.0f);
    }


    protected IEnumerator fadeOut()
    {
        ApplyForceAroundArea();

        yield return null;

        float time = 0.5f;
        while (time > 0.0f)
        {
            time -= Time.deltaTime;
            transform.localScale = Vector3.one * time * diameter * 4.0f;
            yield return null;
        }

        Destroy(gameObject);
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, diameter);
    }

}
