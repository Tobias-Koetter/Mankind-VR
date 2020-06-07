using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceCylinderOnPlane : MonoBehaviour
{
    public GameObject cylinder;
    public MeshFilter spawnArea;
    public GameObject ground;
    public GameObject marker;
    public LineRenderer lR;
    public int rotationNumber;
    private RaycastHit info;
    private KeyCode[] keyCodes;
    public LayerMask mask;

    private MeshFilter CylinderMesh;
    // Start is called before the first frame update
    void Start()
    {
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
                marker.transform.position = RandomPos;
                cylinder.transform.position = info.point;

                cylinder.transform.rotation = Quaternion.FromToRotation(cylinder.transform.up, info.normal);
                RotateCylinderToSide(rotationNumber);
                PushCylinderDown();
                RotateCylinderAtFinalPlace();

                lR.SetPosition(0, info.point + cylinder.transform.up * 10f);
                lR.SetPosition(1, info.point);
                lR.SetPosition(2, info.point + cylinder.transform.forward * 10f);
            }
        }
    }

    public void RotateCylinderToSide(int sideNumber)
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
    public void PushCylinderDown()
    {
        Transform t = cylinder.transform;
        Bounds b = CylinderMesh.sharedMesh.bounds;
        Vector3 extents = b.extents;

        Vector3 up = t.up;
        up = t.InverseTransformDirection(up);
        Vector3 forward = t.forward;
        forward = t.InverseTransformDirection(forward);
        Vector3 right = t.right;
        right = t.InverseTransformDirection(right);
        Vector3 normal = info.normal;
        normal = t.InverseTransformDirection(normal);

        if (rotationNumber == 1)
        {
            //Push object down the negative y Axis
            t.Translate(-up*(extents.y-0.1f), Space.Self);
        }
        if (rotationNumber == 2)
        {
            //Push object down the negative normal direction of placement point on plane (info.normal)
            t.Translate(-normal * (extents.x-0.1f), Space.Self);
        }
        if (rotationNumber == 3)
        {
            //Push object down the positive y axis;
            t.Translate(up * (extents.y - 0.1f), Space.Self);
        }
    }

    public void RotateCylinderAtFinalPlace()
    {
        Vector3 up = cylinder.transform.up;
        up = cylinder.transform.InverseTransformDirection(up);
        Vector3 forward = cylinder.transform.forward;
        forward = cylinder.transform.InverseTransformDirection(forward);
        Vector3 right = cylinder.transform.right;
        right = cylinder.transform.InverseTransformDirection(right);
        Vector3 normal = info.normal;
        normal = cylinder.transform.InverseTransformDirection(normal);

        if (rotationNumber == 2)
        {
            //additionaly Rotate around the local z Axis vector a random degree
            cylinder.transform.Rotate(forward, Random.Range(0, 360));
            
        }
        //Rotate around the local y Axis with a random degree;
        cylinder.transform.Rotate(up, Random.Range(0, 360));


    }
}
