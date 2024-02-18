using System.Net.Http.Headers;
using UnityEngine;

public enum TA_SHAPES {Plane_Small,Plane_Big,Sphere,Cylinder,Cube,NONE};

public class SpawnTrashArea
{
    public LayerMask Mask_ground;
    public LayerMask Mask_smallTrash;
    public LayerMask Mask_trashArea;
    public LayerMask Mask_collision;
    public Spawned[] objects = new Spawned[4];
    public PlaceGeometryOnPlane geometryHandler;

    private RaycastHit[] info;
    private bool hitGround;
    private bool hitTArea;
    private RaycastHit groundHit;

    public SpawnTrashArea(bool inDebug)
    {
        this.Mask_ground = LayerMask.GetMask("Ground");
        this.Mask_smallTrash = LayerMask.GetMask("Trash");
        this.Mask_trashArea = LayerMask.GetMask("TArea");
        this.Mask_collision = Mask_trashArea | Mask_smallTrash | Mask_ground;
        geometryHandler = new PlaceGeometryOnPlane(Mask_ground,inDebug);
    }
    public SpawnTrashArea()
    {
        this.Mask_ground = LayerMask.GetMask("Ground");
        this.Mask_smallTrash = LayerMask.GetMask("Trash");
        this.Mask_trashArea = LayerMask.GetMask("TArea");
        this.Mask_collision = Mask_trashArea | Mask_smallTrash | Mask_ground;
        geometryHandler = new PlaceGeometryOnPlane(Mask_ground);
    }

    public bool Spawn(TA_SHAPES shape, MeshFilter spawnArea,Area_Spawn script, Spawned geometry,int visiblePercentage)
    {
        Vector3 RandomPos = spawnArea.mesh.GetRandomPointInsideConvex();
        RandomPos = spawnArea.transform.TransformPoint(RandomPos);
        info = Physics.RaycastAll(RandomPos, Vector3.down, Mathf.Infinity, Mask_collision);
        hitGround = false;
        hitTArea = false;
        for(int i = 0; i < info.Length; i++)
        {
            int layer = info[i].collider.gameObject.layer;
            if(Mask_ground == (Mask_ground | 1 << layer))
            {
                groundHit = info[i];
                hitGround = true;
            }
            else if(Mask_smallTrash == (Mask_smallTrash | 1 << layer))
            {
                Spawned s = info[i].rigidbody.GetComponent<Spawned>();
                s.SpawnMaster.DespawnObjectWithID(s.poolNumber);
            }
            else if(Mask_trashArea == (Mask_trashArea | 1 << layer))
            {
                Spawned s = info[i].collider.transform.parent.GetComponent<Spawned>();
                if(s.personalTrashValue <8 && geometry.personalTrashValue >=8)
                {
                    //We despawn the TrashArea
                    geometry.SpawnMaster.DespawnTrashArea(s);
                }
                else
                    hitTArea = true; // We block the spawn
            }
        }
        if (hitGround && !hitTArea)
        {
            geometry.currentArea = script;
            script.AddSpawnObject(geometry);

            switch (shape)
            {
                case TA_SHAPES.Cube:
                    geometryHandler.PlaceCube(groundHit.point, groundHit.normal, geometry,visiblePercentage);
                    break;
                case TA_SHAPES.Cylinder:
                    geometryHandler.PlaceCylinder(groundHit.point, groundHit.normal, geometry,visiblePercentage);
                    break;
                case TA_SHAPES.Plane_Small:
                case TA_SHAPES.Plane_Big:
                    geometryHandler.PlacePlane(groundHit.point, groundHit.normal, geometry,visiblePercentage);
                    break;
                case TA_SHAPES.Sphere:
                    geometryHandler.PlaceSphere(groundHit.point, groundHit.normal, geometry,visiblePercentage);
                    break;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Spawn(TA_SHAPES shape, MeshFilter spawnArea, Area_Spawn script, Spawned geometry) => Spawn(shape, spawnArea, script, geometry, 30);
}
