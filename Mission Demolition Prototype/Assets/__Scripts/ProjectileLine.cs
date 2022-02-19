using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //Single object

    [Header("Set in inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; //Link on the single object
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject Poi { 
        get 
        { 
            return (_poi); 
        } 
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //If point not far enough from last point, just exit
            return;
        }

        if (points.Count == 0) //If this launch point
        {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; //optionally fragment of line that help aiming in future
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;

            //set first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }

        else
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //getting location last added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }
            return points[points.Count - 1];
        }
    }

    private void FixedUpdate()
    {
        if (Poi == null)
        {
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.CompareTag("Projectile"))
                {
                    Poi = FollowCam.POI;
                }

                else
                {
                    return; //Exit, if POI is dont find
                }
            }

            else
            {
                return; //Exit, if POI is dont find
            }
        }

        //If POI finded
        AddPoint();
        if (FollowCam.POI == null)
        {
            Poi = null;
        }
    }
}
