using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour {


    public static CameraCtrl instance;
    public static Camera cmr;

    public Player target;


    public Vector2 forwardxz = Vector2.up;
    public Vector3 forward
    {
        get
        {
            return new Vector3(forwardxz.x, 0, forwardxz.y);
        }
    }
    public Vector3 right
    {
        get
        {
            return new Vector3(forwardxz.y, 0, -forwardxz.x);
        }
    }
    public float upAngle = 0.0f;

    public Vector3 normal = Vector3.forward;

    public bool cameraLock = true;

    protected void Start()
    {
        instance = this;
        cmr = GetComponentInChildren<Camera>();

        if (cameraLock)
            Cursor.lockState = CursorLockMode.Locked;
        SetPlayer(target);
    }

    protected void Update()
    {
        HandleInput();
        if (cameraLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    protected void FixedUpdate()
    {
        RotateCamera();
    }
    protected void LateUpdate()
    {
        FollowPlayer();
    }


    protected void RotateCamera()
    {
        if (cameraLock)
        {
            Vector2 m = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            //normal = Quaternion.AngleAxis(Mathf.Pow(m.y, 1.5f) * -8.0f * Time.deltaTime, new Vector3(normal.z + 0.01f, 0, -normal.x).normalized) * normal;
            forwardxz = Quaternion.AngleAxis(m.x * -75.0f  * Time.deltaTime, Vector3.forward) * forwardxz;
            upAngle = Mathf.Clamp(upAngle + m.y * -120.0f * (m.y > 0 ? -1 : -1) * Time.deltaTime, -89.0f, 89.0f);

            float xzWeight = upAngle / 90.0f;
            normal = new Vector3(forwardxz.x * (1.0f - Mathf.Pow(xzWeight, 2)), xzWeight, forwardxz.y * (1.0f - Mathf.Pow(xzWeight, 2)));

            transform.rotation = Quaternion.LookRotation(normal);
        }
    }
    protected void HandleInput()
    {
        if (Input.GetMouseButtonDown(2))
        {
            cameraLock = !cameraLock;
        }
    }
    protected void FollowPlayer()
    {
        if (target != null)
        {
            transform.position = target.transform.position;
        }
    }

    public void SetPlayer(Player pl)
    {
        if (pl == null)
            return;

        target = pl;
        Player.cmrCtrl = this;
        Player.cmr = cmr;
    }


}
