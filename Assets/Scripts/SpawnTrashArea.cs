using UnityEngine;

public enum TA_SHAPES {Sphere,Cube,Plane,Cylinder};

public class SpawnTrashArea : MonoBehaviour
{

    [Header("needed Information")]
    public MeshFilter spawnArea;
    public LayerMask mask;

    [Header("sphere,cube,plane,cylinder")]
    public Spawned[] objects = new Spawned[4];
    
    [Header("Can be left Empty or use predefined Object")]
    public PlaceGeometryOnPlane geometryHandler;

    private RaycastHit info;

    private KeyCode[] codes;
    // Start is called before the first frame update
    void Start()
    {
        codes = new KeyCode[] {KeyCode.Alpha1,KeyCode.Alpha2,KeyCode.Alpha3, KeyCode.Alpha4};
        if (geometryHandler)
            geometryHandler.SetUp(mask);
        else
            geometryHandler = new PlaceGeometryOnPlane(mask);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown)
        {
            TA_SHAPES current = TA_SHAPES.Sphere;
            for (int i = 0; i<codes.Length; i++)
            {
                
                if(Input.GetKeyDown(codes[i]))
                {
                    Spawn(current + i, spawnArea, objects[i]);
                }
            }
        }
    }

    public void Spawn(TA_SHAPES shape, MeshFilter spawnArea, Spawned geometry)
    {
        foreach(Spawned s in objects)
        {
            s.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        Vector3 RandomPos = spawnArea.mesh.GetRandomPointInsideConvex();
        RandomPos = spawnArea.transform.TransformPoint(RandomPos);

        if (Physics.Raycast(RandomPos, Vector3.down, out info, Mathf.Infinity, mask))
        {
            switch(shape)
            {
                case TA_SHAPES.Cube:
                    geometryHandler.PlaceCube(info.point, info.normal, geometry);
                    break;
                case TA_SHAPES.Cylinder:
                    geometryHandler.PlaceCylinder(info.point, info.normal, geometry);
                    break;
                case TA_SHAPES.Plane:
                    geometryHandler.PlacePlane(info.point, info.normal, geometry);
                    break;
                case TA_SHAPES.Sphere:
                    geometryHandler.PlaceSphere(info.point, info.normal, geometry);
                    break;
            }
        }
    }
}
