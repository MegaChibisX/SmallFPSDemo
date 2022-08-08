using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {



    public float time = 1.0f;
	
	void Update ()
    {
        time -= Time.deltaTime;
        if (time <= 0.0f)
            Destroy(gameObject);
	}
}
