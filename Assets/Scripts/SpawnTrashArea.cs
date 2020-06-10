using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TA_SHAPES {Sphere,Cube,Plane,Cylinder};

public class SpawnTrashArea : MonoBehaviour
{

    [Header("needed Information")]
    public MeshFilter spawnArea;
    public GameObject sphere;
    public GameObject cube;
    public GameObject cylinder;
    public LayerMask mask;

    private PlaceGeometryOnPlane geometryHandler;
    private RaycastHit info;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn(TA_SHAPES shape, MeshFilter spawnArea)
    {
        cube.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        sphere.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        cylinder.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        Vector3 RandomPos = spawnArea.mesh.GetRandomPointInsideConvex();
        RandomPos = spawnArea.transform.TransformPoint(RandomPos);

        if (Physics.Raycast(RandomPos, Vector3.down, out info, Mathf.Infinity, mask))
        {
            switch(shape)
            {
                case TA_SHAPES.Cube:
                    geometryHandler.PlaceCube(info.point, info.normal);
                    break;
                case TA_SHAPES.Cylinder:
                    geometryHandler.PlaceCylinder(info.point, info.normal);
                    break;
                case TA_SHAPES.Plane:
                    break;
                case TA_SHAPES.Sphere:
                    geometryHandler.PlaceSphere(info.point, info.normal);
                    break;
            }
        }
    }
}
