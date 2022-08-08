using System.Collections.Generic;
using UnityEngine;

public class Weapon3 : Weapon
{

    public List<Weapon3_Missile> waitingMissiles;
    public List<Vector3> missileTargets;


    private AnimationCurve zoomFast;
    private AnimationCurve zoomSlow;

    public Weapon3(Player _player) : base(_player)
    {

    }

    public override void Start()
    {
        waitingMissiles = new List<Weapon3_Missile>();
        missileTargets = new List<Vector3>();

        zoomFast = new AnimationCurve();
        zoomFast.AddKey(0.0f, 1.0f);
        zoomFast.AddKey(0.5f, 0.5f);
        zoomFast.AddKey(0.7f, 0.5f);
        zoomFast.AddKey(1.3f, 0.8f);
        zoomFast.AddKey(2.0f, 1.0f);
        zoomFast.AddKey(3.0f, 1.0f);

        zoomSlow = new AnimationCurve();
        zoomSlow.AddKey(0.0f, 1.0f);
        zoomSlow.AddKey(0.2f, 1.0f);
        zoomSlow.AddKey(0.7f, 0.5f);
        zoomSlow.AddKey(1.3f, 1.0f);
        zoomSlow.AddKey(2.0f, 1.0f);
    }
    public override void Update()
    {
        HandleDisplay();
    }



    public override void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
            Fire1();
        else if (Input.GetMouseButtonDown(1))
            Fire2();
    }

    protected override void Fire1()
    {
        CreateMissile();
    }
    protected override void Fire2()
    {
        if (waitingMissiles.Count == 0)
            CreateMissile();
        LaunchWaitingMissiles();
    }


    protected void CreateMissile()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(ray, out hit))
            missileTargets.Add(hit.point);

        GameObject shot = Object.Instantiate(player.wpnGui.W3_Missile.gameObject);
        shot.transform.position = player.transform.position;
        shot.transform.rotation = Quaternion.LookRotation(ray.direction);
        shot.GetComponent<Attack>().owner = player;
        waitingMissiles.Add(shot.GetComponent<Weapon3_Missile>());
    }
    protected void LaunchWaitingMissiles()
    {
        foreach (Weapon3_Missile mis in waitingMissiles)
        {
            mis.Launch();
        }
        waitingMissiles.Clear();
        missileTargets.Clear();
    }

    public override void HandleDisplay()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = CameraCtrl.cmr.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        Physics.Raycast(ray, out hit);

        float curDistance = hit.distance;
        if (curDistance == 0.0f || hit.rigidbody == null)
            curDistance = 1.0f;
        else
        {
            curDistance = Mathf.Clamp(100.0f - curDistance, 10.0f, 100.0f) * 0.01f;
        }

        _curDistance = curDistance;
        _smoothDistance = Mathf.Lerp(_smoothDistance, _curDistance, Time.deltaTime * 10.0f);
    }



    public override void DisplayCrosshair(float x, float y, bool simple)
    {
        if (simple)
        {
            if (player.wpnGui.W1_crossHairFull != null)
                GUI.DrawTexture(new Rect(x * 14, y * 16 - x * _size * 0.5f, x * _size, x * _size), player.wpnGui.W1_crossHairFull);
        }
        else
        {
            float multi = _size;


            Matrix4x4 defaultMatrix = GUI.matrix;
            multi = _size + zoomSlow.Evaluate((Time.time * 1.0f) % 2.0f);

            GUIUtility.RotateAroundPivot(Mathf.Cos(Time.time * 2.0f) * 10.0f, new Vector2(x * 16, y * 16));

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f * _smoothDistance, y * 16 - x * multi * 0.5f * _smoothDistance,
                                     x * multi * _smoothDistance, x * multi * _smoothDistance), player.wpnGui.W3_ZoomSlow);

            GUI.matrix = defaultMatrix;

            multi = _size + zoomFast.Evaluate((Time.time * 1.0f) % 2.0f);

            GUIUtility.RotateAroundPivot(Mathf.Cos(Time.time * 2.0f) * 10.0f, new Vector2(x * 16, y * 16));
            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f * _smoothDistance, y * 16 - x * multi * 0.5f * _smoothDistance,
                         x * multi * _smoothDistance, x * multi * _smoothDistance), player.wpnGui.W3_Zoom);

            GUI.matrix = defaultMatrix;

            multi = _size;

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                         x * multi, x * multi), player.wpnGui.W3_Static);

            multi = _size * _smoothDistance;

            GUI.DrawTexture(new Rect(x * 16 - x * multi * 0.5f, y * 16 - x * multi * 0.5f,
                         x * multi, x * multi), player.wpnGui.W3_SubTarget);

            multi = _size;

            foreach (Vector3 v in missileTargets)
            {
                Vector3 a = v - CameraCtrl.cmr.transform.position;
                if (Vector3.Angle(a, CameraCtrl.cmr.transform.forward) < 60.0f)
                {
                    Vector3 p = CameraCtrl.cmr.WorldToScreenPoint(v);
                    GUI.DrawTexture(new Rect(p.x - x * multi * 0.5f, Screen.height - p.y - x * multi * 0.5f,
                                             x * multi, x * multi), player.wpnGui.W3_SubTarget);
                }
            }

            if (Event.current.type.Equals(EventType.Repaint))
            {
                Graphics.DrawTexture(new Rect(x * 16 - x * multi * 0.5f * _hitDistance, y * 16 - x * multi * 0.5f * _hitDistance,
                         x * multi * _hitDistance, x * multi * _hitDistance),
                         player.wpnGui.W1_hitTarget, player.wpnGui.hitMaterial);
            }
        }
    }

}
