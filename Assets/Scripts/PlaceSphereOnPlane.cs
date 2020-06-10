using UnityEngine;

public class PlaceSphereOnPlane : MonoBehaviour
{
    [Header("will be moved to the calling Script")]
    public MeshFilter spawnArea;

    [Header("needed Information")]
    public GameObject sphere;
    public LayerMask mask;

    // only for debug
    [Header("Debug Options")]
    public bool inDebug = false;
    public GameObject marker;
    public LineRenderer lR;
    // end of debug components

    private RaycastHit info;
    private MeshFilter sphereMesh;


    // Start is called before the first frame update
    void Start()
    {
        if(inDebug)
        {
            marker.SetActive(true);
            lR.enabled = true;
        }


        try
        {
            sphereMesh = sphere.GetComponent<MeshFilter>();
        }
        catch (System.Exception e)
        {
            Debug.LogError("The Structure of the cylinder object has changed! |->|" + e);
            sphereMesh = sphere.GetComponentInChildren<MeshFilter>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            sphere.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

            Vector3 RandomPos = spawnArea.mesh.GetRandomPointInsideConvex();
            RandomPos = spawnArea.transform.TransformPoint(RandomPos);

            if (Physics.Raycast(RandomPos, Vector3.down, out info, Mathf.Infinity, mask))
            {
                if (inDebug){marker.transform.position = RandomPos;}

                PlaceSphere(info.point, info.normal);
            }
        }
    }
    /// <summary>
    /// Place a Sphere at a specific Point and add a random rotation to it.
    /// </summary>
    /// <param name="pointOnPlane"> 
    /// The given Position on the plane
    /// </param>
    /// <param name="normalOfPlane"> 
    /// The normal of the plane at the given postion.
    /// It is needed to initialize the rotation of the Sphere 
    /// at the beginning of the placement process (the Y-Axis gets alligned to the normal vector)
    /// </param>
    public void PlaceSphere(Vector3 pointOnPlane, Vector3 normalOfPlane)
    {
        sphere.transform.position = pointOnPlane;
        sphere.transform.rotation = Quaternion.FromToRotation(sphere.transform.up, normalOfPlane);

        PushSphereDown();
        RotateSphereAtFinalPlace();

        if (inDebug)
        {
            lR.SetPosition(0, pointOnPlane + sphere.transform.up * 10f);
            lR.SetPosition(1, pointOnPlane);
            lR.SetPosition(2, pointOnPlane + sphere.transform.forward * 10f);
        }
    }

    /// <summary>
    /// Push Sphere down, so only a specific part is visible in the world
    /// </summary>
    private void PushSphereDown()
    {
        Transform t = sphere.transform;
        Bounds b = sphereMesh.sharedMesh.bounds;
        Vector3 extents = b.extents;

        Vector3 up = t.up;
        up = t.InverseTransformDirection(up);

        // Push down the negative Y-Axis
        t.Translate(-up * (extents.y - 0.2f), Space.Self);
    }

    /// <summary>
    /// After the Sphere Object is at its final postion on the plane, add some random rotation to its visible part
    /// </summary>
    private void RotateSphereAtFinalPlace()
    {
        // set local rotation to a randomly generated rotation
        sphere.transform.localRotation = Random.rotationUniform;
        

    }
}