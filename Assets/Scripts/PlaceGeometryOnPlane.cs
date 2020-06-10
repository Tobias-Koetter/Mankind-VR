using UnityEngine;


public class PlaceGeometryOnPlane : MonoBehaviour
{
    [Header("will be moved to the calling Script")]
    public MeshFilter spawnArea;

    [Header("needed Information")]
    public GameObject sphere;
    public GameObject cube;
    public GameObject cylinder;
    public LayerMask mask;

    // only for debug
    [Header("Debug Options")]
    public bool inDebug = false;
    public GameObject marker;
    public LineRenderer lR;
    // end of debug components

    private MeshFilter sphereMesh;
    private MeshFilter cubeMesh;
    private MeshFilter cylinderMesh;
    private Vector3 curNormal;
    public int rotationNumber;

    public PlaceGeometryOnPlane(GameObject sphere, GameObject cube, GameObject cylinder)
    {
        this.sphere = sphere;
        this.cube = cube;
        this.cylinder = cylinder;

        sphereMesh = sphere.GetComponent<MeshFilter>();
        cubeMesh = cube.GetComponent<MeshFilter>();
        cylinderMesh = cylinder.GetComponent<MeshFilter>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region PlaceSphere
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
    
    #endregion

    #region PlaceCube
    public void PlaceCube(Vector3 pointOnPlane, Vector3 normalOfPlane)
    {
        cube.transform.position = pointOnPlane;
        cube.transform.rotation = Quaternion.FromToRotation(cube.transform.up, normalOfPlane);
        rotationNumber = Random.Range(1, 7);

        RotateCubeToSide();
        PushCubeDown();
        RotateCubeAtFinalPlace();

        if (inDebug)
        {
            lR.SetPosition(0, pointOnPlane + cube.transform.up * 10f);
            lR.SetPosition(1, pointOnPlane);
            lR.SetPosition(2, pointOnPlane + cube.transform.forward * 10f);
        }
    }


    private void RotateCubeToSide()
    {
        Vector3 forward = cube.transform.forward;
        forward = cube.transform.InverseTransformDirection(forward);
        Vector3 right = cube.transform.right;
        right = cube.transform.InverseTransformDirection(right);

        if (rotationNumber == 1)
        {
            // Do Nothing
        }
        else if (rotationNumber == 2)
        {
            cube.transform.Rotate(right, -90f);
        }
        else if (rotationNumber == 3)
        {
            cube.transform.Rotate(forward, 90f);
        }
        else if (rotationNumber == 4)
        {
            cube.transform.Rotate(right, 90f);
        }
        else if (rotationNumber == 5)
        {
            cube.transform.Rotate(forward, -90f);
        }
        else if (rotationNumber == 6)
        {
            cube.transform.Rotate(forward, 180f);
        }
        lR.SetPosition(0, cube.transform.position);
        lR.SetPosition(1, cube.transform.position + cube.transform.up * 10f);
    }

    private void PushCubeDown()
    {
        Transform t = cube.transform;
        Bounds b = cubeMesh.sharedMesh.bounds;
        Vector3 extents = b.extents;

        Vector3 up = t.up;
        up = t.InverseTransformDirection(up);
        Vector3 forward = t.forward;
        forward = t.InverseTransformDirection(forward);
        Vector3 right = t.right;
        right = t.InverseTransformDirection(right);

        if (rotationNumber == 1)
        {
            // Push down the neg y Axis
            t.Translate(-up * (extents.y - 0.1f), Space.Self);
        }
        else if (rotationNumber == 2)
        {
            // Push down the neg z Axis
            t.Translate(-forward * (extents.z - 0.1f), Space.Self);
        }
        else if (rotationNumber == 3)
        {
            // Push down the neg x Axis
            t.Translate(-right * (extents.x - 0.1f), Space.Self);
        }
        else if (rotationNumber == 4)
        {
            // Push down the pos z Axis
            t.Translate(forward * (extents.z - 0.1f), Space.Self);
        }
        else if (rotationNumber == 5)
        {
            // Push down the pos x Axiz
            t.Translate(right * (extents.x - 0.1f), Space.Self);
        }
        else if (rotationNumber == 6)
        {
            // Push down the pos y Axiz
            t.Translate(up * (extents.y - 0.1f), Space.Self);
        }
    }

    private void RotateCubeAtFinalPlace()
    {
        Transform t = cube.transform;

        Vector3 up = t.up;
        up = t.InverseTransformDirection(up);
        Vector3 forward = t.forward;
        forward = t.InverseTransformDirection(forward);
        Vector3 right = t.right;
        right = t.InverseTransformDirection(right);

        if (rotationNumber == 1 || rotationNumber == 6)
        {
            // Randomly Rotate around local Y-Axis
            t.Rotate(up, Random.Range(0, 360));

        }
        else if (rotationNumber == 2 || rotationNumber == 4)
        {
            // Randomly Rotate around local Z-Axis
            t.Rotate(forward, Random.Range(0, 360));

        }
        else if (rotationNumber == 3 || rotationNumber == 5)
        {
            // Randomly Rotate around local X-Axis
            t.Rotate(right, Random.Range(0, 360));

        }


    }
    #endregion

    #region PlaceCylinder

    public void PlaceCylinder(Vector3 pointOnPlane, Vector3 normalOfPlane)
    {
        cylinder.transform.position = pointOnPlane;
        cylinder.transform.rotation = Quaternion.FromToRotation(cylinder.transform.up, normalOfPlane);

        curNormal = normalOfPlane;
        rotationNumber = Random.Range(1, 4);

        RotateCylinderToSide();
        PushCylinderDown();
        RotateCylinderAtFinalPlace();

        if (inDebug)
        {
            lR.SetPosition(0, pointOnPlane + cylinder.transform.up * 10f);
            lR.SetPosition(1, pointOnPlane);
            lR.SetPosition(2, pointOnPlane + cylinder.transform.forward * 10f);
        }
    }

    private void RotateCylinderToSide()
    {
        Vector3 forward = cylinder.transform.forward;
        forward = cylinder.transform.InverseTransformDirection(forward);
        Vector3 right = cylinder.transform.right;
        right = cylinder.transform.InverseTransformDirection(right);

        if (rotationNumber == 1)
        {
            // Do Nothing
        }
        else if (rotationNumber == 2)
        {
            cylinder.transform.Rotate(right, -90f);
        }
        else if (rotationNumber == 3)
        {
            cylinder.transform.Rotate(right, 180f);
        }


    }

    private void PushCylinderDown()
    {
        Transform t = cylinder.transform;
        Bounds b = cylinderMesh.sharedMesh.bounds;
        Vector3 extents = b.extents;

        Vector3 up = t.up;
        up = t.InverseTransformDirection(up);
        Vector3 normal = curNormal;
        normal = t.InverseTransformDirection(normal);

        if (rotationNumber == 1)
        {
            //Push object down the negative y Axis
            t.Translate(-up * (extents.y - 0.1f), Space.Self);
        }
        if (rotationNumber == 2)
        {
            //Push object down the negative normal direction of placement point on plane
            t.Translate(-normal * (extents.x - 0.1f), Space.Self);
        }
        if (rotationNumber == 3)
        {
            //Push object down the positive y axis;
            t.Translate(up * (extents.y - 0.1f), Space.Self);
        }
    }

    private void RotateCylinderAtFinalPlace()
    {
        Vector3 up = cylinder.transform.up;
        up = cylinder.transform.InverseTransformDirection(up);
        Vector3 forward = cylinder.transform.forward;
        forward = cylinder.transform.InverseTransformDirection(forward);

        if (rotationNumber == 2)
        {
            //additionaly Rotate around the local z Axis vector a random degree
            cylinder.transform.Rotate(forward, Random.Range(0, 360));

        }
        //Rotate around the local y Axis with a random degree;
        cylinder.transform.Rotate(up, Random.Range(0, 360));
    }


    #endregion

}
