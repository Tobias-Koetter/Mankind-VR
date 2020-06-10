using System;
using UnityEngine;
using UnityEngine.ProBuilder;
using Random = UnityEngine.Random;

public class PlaceCylinderOnPlane : MonoBehaviour
{
    [Header("will be moved to the calling Script")]
    public MeshFilter spawnArea;

    [Header("needed Information")]
    public GameObject cylinder;
    public LayerMask mask;

    // only for debug
    [Header("Debug Options")]
    public bool inDebug = false;
    public GameObject marker;
    public LineRenderer lR;
    // end of debug components

    private MeshFilter CylinderMesh;
    private RaycastHit info;
    private Vector3 curNormal;
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
            CylinderMesh = cylinder.GetComponent<MeshFilter>();
        }
        catch(Exception e)
        {
            Debug.LogError("The Structure of the cylinder object has changed! |->|"+e);
            CylinderMesh = cylinder.GetComponentInChildren<MeshFilter>();
        }
        keyCodes = new KeyCode[]
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
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
            cylinder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            Vector3 RandomPos = spawnArea.mesh.GetRandomPointInsideConvex();
            RandomPos = spawnArea.transform.TransformPoint(RandomPos);
            if(Physics.Raycast(RandomPos,Vector3.down,out info,Mathf.Infinity,mask))
            {
                if (inDebug) { marker.transform.position = RandomPos; }


                SpawnCylinder(info.point, info.normal);

            }
        }
    }

    public void SpawnCylinder(Vector3 pointOnPlane, Vector3 normalOfPlane)
    {
        cylinder.transform.position = pointOnPlane;
        cylinder.transform.rotation = Quaternion.FromToRotation(cylinder.transform.up, normalOfPlane);

        curNormal = normalOfPlane;

        RotateCylinderToSide(rotationNumber);
        PushCylinderDown();
        RotateCylinderAtFinalPlace();

        if (inDebug)
        {
            lR.SetPosition(0, pointOnPlane + cylinder.transform.up * 10f);
            lR.SetPosition(1, pointOnPlane);
            lR.SetPosition(2, pointOnPlane + cylinder.transform.forward * 10f);
        }
    }

    private void RotateCylinderToSide(int sideNumber)
    {
        Vector3 forward = cylinder.transform.forward;
        forward = cylinder.transform.InverseTransformDirection(forward);
        Vector3 right = cylinder.transform.right;
        right = cylinder.transform.InverseTransformDirection(right);

        if (sideNumber == 1)
        {
            // Do Nothing
        }
        else if (sideNumber == 2)
        {
            cylinder.transform.Rotate(right, -90f);
        }
        else if (sideNumber == 3)
        {
            cylinder.transform.Rotate(right, 180f);
        }
        

    }

    private void PushCylinderDown()
    {
        Transform t = cylinder.transform;
        Bounds b = CylinderMesh.sharedMesh.bounds;
        Vector3 extents = b.extents;

        Vector3 up = t.up;
        up = t.InverseTransformDirection(up);
        Vector3 normal = curNormal;
        normal = t.InverseTransformDirection(normal);

        if (rotationNumber == 1)
        {
            //Push object down the negative y Axis
            t.Translate(-up*(extents.y-0.1f), Space.Self);
        }
        if (rotationNumber == 2)
        {
            //Push object down the negative normal direction of placement point on plane
            t.Translate(-normal * (extents.x-0.1f), Space.Self);
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
}
