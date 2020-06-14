using UnityEngine;

public enum TA_SHAPES {Plane_Small,Plane_Big,Sphere,Cylinder,Cube};

public class SpawnTrashArea
{
    public LayerMask mask;
    public Spawned[] objects = new Spawned[4];
    public PlaceGeometryOnPlane geometryHandler;

    private RaycastHit info;

    public SpawnTrashArea()
    {
        this.mask = LayerMask.GetMask("Ground");
        geometryHandler = new PlaceGeometryOnPlane(mask);
    }

    public void Spawn(TA_SHAPES shape, MeshFilter spawnArea, Spawned geometry)
    {

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
                case TA_SHAPES.Plane_Small:
                case TA_SHAPES.Plane_Big:
                    geometryHandler.PlacePlane(info.point, info.normal, geometry);
                    break;
                case TA_SHAPES.Sphere:
                    geometryHandler.PlaceSphere(info.point, info.normal, geometry);
                    break;
            }
        }
    }
}
