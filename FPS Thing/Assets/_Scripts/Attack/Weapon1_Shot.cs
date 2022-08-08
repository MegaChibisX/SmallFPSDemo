using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1_Shot : Attack {

    public Rigidbody visualShot;
    public GameObject explosionPrefab;
    public LineRenderer line;
    protected GameObject ray;

    protected Vector3 startPos;
    protected RaycastHit hit;

    public float distance = 1.0f;


    protected void Start()
    {
        startPos = transform.position - transform.forward * (500.0f + distance) * Time.fixedDeltaTime;
    }
    protected void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            line.transform.localScale = Vector3.one * hit.distance;
        }
        else
        {
            line.transform.localScale = Vector3.one * 1000.0f;
        }
    }
    protected void FixedUpdate()
    {
        Vector3 newPos = visualShot.transform.position + transform.forward * (500.0f + distance) * Time.fixedDeltaTime;
        if (Physics.Raycast(visualShot.transform.position, transform.forward, out hit, (500.0f + distance) * Time.fixedDeltaTime))
        {
            Destroy(visualShot.gameObject);
            explosionPrefab = Instantiate(explosionPrefab);
            explosionPrefab.transform.position = hit.point;
            explosionPrefab.transform.rotation = Quaternion.LookRotation(hit.normal);
            explosionPrefab.transform.localScale = Vector3.Scale(explosionPrefab.transform.localScale, transform.localScale);
            Destroy(gameObject);
        }
        else
        {
            visualShot.MovePosition(newPos);
            line.transform.position = startPos;
        }
    }


}
