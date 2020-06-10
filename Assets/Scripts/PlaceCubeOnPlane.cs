using UnityEngine;

public class PlaceCubeOnPlane : MonoBehaviour
{
    [Header("will be moved to the calling Script")]
    public MeshFilter spawnArea;

    [Header("needed Information")]
    public GameObject cube;
    public LayerMask mask;

    // only for debug
    [Header("Debug Options")]
    public bool inDebug = false;
    public GameObject marker;
    public LineRenderer lR;
    // end of debug components

    private MeshFilter cubeMesh;
    private RaycastHit info;
    private KeyCode[] keyCodes;
    
    public int rotationNumber;

    // Start is called before the first frame update
    void Start()
    {
        if (inDebug)
        {
            marker.SetActive(true);
            lR.enabled = true;
        }


        try
        {
            cubeMesh = cube.GetComponent<MeshFilter>();
        }
        catch (System.Exception e)
        {
            Debug.LogError("The Structure of the cylinder object has changed! |->|" + e);
            cubeMesh = cube.GetComponentInChildren<MeshFilter>();
        }

        keyCodes = new KeyCode[]
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
        };
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            for (int i = 0; i < keyCodes.Length; i++)
            {
                if (Input.GetKeyDown(keyCodes[i]))
                {
                    rotationNumber = i + 1;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            cube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            Vector3 RandomPos = spawnArea.mesh.GetRandomPointInsideConvex();
            RandomPos = spawnArea.transform.TransformPoint(RandomPos);
            if(Physics.Raycast(RandomPos,Vector3.down,out info,Mathf.Infinity,mask))
            {
                if (inDebug) { marker.transform.position = RandomPos; }

                PlaceCube(info.point, info.normal);

            }
        }
    }

    public void PlaceCube(Vector3 pointOnPlane, Vector3 normalOfPlane)
    {
        cube.transform.position = pointOnPlane;
        cube.transform.rotation = Quaternion.FromToRotation(cube.transform.up, normalOfPlane);

        RotateCubeToSide(rotationNumber);
        PushCubeDown();
        RotateCubeAtFinalPlace();

        if (inDebug)
        {
            lR.SetPosition(0, pointOnPlane + cube.transform.up * 10f);
            lR.SetPosition(1, pointOnPlane);
            lR.SetPosition(2, pointOnPlane + cube.transform.forward * 10f);
        }
    }


    private void RotateCubeToSide(int sideNumber)
    {
        Vector3 forward = cube.transform.forward;
        forward = cube.transform.InverseTransformDirection(forward);
        Vector3 right = cube.transform.right;
        right = cube.transform.InverseTransformDirection(right);
        if (sideNumber == 1)
        {
            // Do Nothing
        }
        else if (sideNumber == 2)
        {
            cube.transform.Rotate(right, -90f);
        }
        else if (sideNumber == 3)
        {
            cube.transform.Rotate(forward, 90f);
        }
        else if (sideNumber == 4)
        {
            cube.transform.Rotate(right, 90f);
        }
        else if (sideNumber == 5)
        {
            cube.transform.Rotate(forward, -90f);
        }
        else if (sideNumber == 6)
        {
            cube.transform.Rotate(forward, 180f);
        }
        lR.SetPosition(0, cube.transform.position);
        lR.SetPosition(1, cube.transform.position + cube.transform.up*10f);
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
}
