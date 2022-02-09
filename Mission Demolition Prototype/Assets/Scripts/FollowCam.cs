using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dinamically")]
    public float camZ;

    private void Awake()
    {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate()
    {
        if (POI == null) return; //exit, if dont have necessary object

        //get position of necessary object
        Vector3 destination = POI.transform.position;

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //determine the point between current location and destination
        destination = Vector3.Lerp(transform.position, destination, easing);

        destination.z = camZ;
        //set cam in position destination
        transform.position = destination;

        Camera.main.orthographicSize = destination.y + 10;
    }
}
