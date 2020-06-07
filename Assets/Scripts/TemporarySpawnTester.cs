using UnityEngine;

public enum TRASHAREA_FORM { PLANE_SMALL,PLANE_BIG,SPHERE,BARROL,CUBE}
public struct TrashArea
{
    public GameObject own;
    public TRASHAREA_FORM form;
    public Mesh body;
    public MeshFilter filter;

    public TrashArea(GameObject own, TRASHAREA_FORM form)
    {
        this.own = own;
        this.form = form;
        this.filter = own.GetComponentInChildren<MeshFilter>();
        this.body = filter.sharedMesh;
        Debug.Log("own: " + own + "\nform: " + form + "\nbody: " + body+" with extents: "+body.bounds.ToString("F4"));
    }
};

public class TemporarySpawnTester : MonoBehaviour
{
    [Header("TrashArea Objects")]
    public GameObject Trash1;
    public GameObject Trash2;
    public GameObject Trash3;
    public GameObject Trash4;
    public GameObject Trash5;

    [Header("Ground Information")]
    public MeshFilter ground;
    public LayerMask toHit;
    public LineRenderer lR;

    private bool isPlacingTrash = false;
    private TrashArea[] tAs;

    private GameObject current = null;
    private TrashArea currentInfo;
    private Vector3 curPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (Trash1 && Trash2 && Trash3 && Trash4 && Trash5)
        {
            tAs = new TrashArea[5];
            tAs[0] = new TrashArea(Trash1,TRASHAREA_FORM.PLANE_SMALL);
            tAs[1] = new TrashArea(Trash2, TRASHAREA_FORM.PLANE_BIG);
            tAs[2] = new TrashArea(Trash3, TRASHAREA_FORM.SPHERE);
            tAs[3] = new TrashArea(Trash4, TRASHAREA_FORM.BARROL);
            tAs[4] = new TrashArea(Trash5, TRASHAREA_FORM.CUBE);
        }
        else
        {
            Debug.LogError("The Trashmodels aren't set yet.");
            Debug.Break();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlacingTrash && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                current = Instantiate<GameObject>(tAs[0].own);
                currentInfo = tAs[0];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                current = Instantiate<GameObject>(tAs[1].own);
                currentInfo = tAs[1];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                current = Instantiate<GameObject>(tAs[2].own);
                currentInfo = tAs[2];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                current = Instantiate<GameObject>(tAs[3].own);
                currentInfo = tAs[3];
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                current = Instantiate<GameObject>(tAs[4].own);
                currentInfo = tAs[4];
            }

            if(current)
            {
                current.SetActive(false);
                isPlacingTrash = true;
            }
        }
        else if(isPlacingTrash)
        {
            if (curPos == Vector3.zero)
            {
                curPos =  ground.mesh.GetRandomPointInsideConvex();
                curPos = ground.transform.TransformPoint(curPos);
            }
            else
            {
                
                RaycastHit info = new RaycastHit();
                if(Physics.Raycast(curPos, Vector3.down, out info, Mathf.Infinity, toHit))
                {
                    curPos = info.point;
                    lR.SetPosition(0, curPos);
                    lR.SetPosition(1, curPos + info.normal * 10f);
                    lR.enabled = true;
                    RandomizePlacement(info);
                }
                current.SetActive(true);
                current = null;
                curPos = Vector3.zero;
                isPlacingTrash = false;
            }
            
        }
    }

    private void RandomizePlacement(RaycastHit info)
    {
        Vector3 curNormal;
        int rot;
        switch (currentInfo.form)
        {
            case TRASHAREA_FORM.PLANE_SMALL:
            case TRASHAREA_FORM.PLANE_BIG:
                rot = Random.Range(0, 360);
                current.transform.Rotate(current.transform.up, rot);
                curNormal = current.transform.up;
                current.transform.rotation = Quaternion.FromToRotation(curNormal, info.normal) * current.transform.rotation;
                current.transform.position = curPos;
                break;
            case TRASHAREA_FORM.SPHERE:
                current.transform.rotation = Random.rotation;
                current.transform.position = curPos;
                current.transform.Translate(0f, -1f, 0f, Space.World);
                break;
            case TRASHAREA_FORM.BARROL:
                int areaToShow = Random.Range(1, 4);
                if(areaToShow == 2)
                {
                    curNormal = current.transform.forward;
                    current.transform.localRotation = Quaternion.FromToRotation(curNormal, info.normal) * current.transform.localRotation;
                    
                    current.transform.position = curPos;
                    rot = Random.Range(0, 360);
                    current.transform.RotateAround(curPos, info.normal, rot);
                    rot = Random.Range(0, 360);
                    current.transform.RotateAround(curPos, current.transform.up, rot);

                    Vector3 extents = currentInfo.body.bounds.extents;
                    extents = Vector3.Scale(extents, currentInfo.filter.transform.localScale);
                    Debug.Log(info.normal+"normal altered "+ info.normal * -1 * 5f + "areaToShow" + areaToShow);
                    Debug.Log("current.transform.position: " + current.transform.position + " before");
                    current.transform.Translate( info.normal * -1 * 5f);
                    Debug.Log("current.transform.position: " + current.transform.position + " after");


                }
                else if(areaToShow == 1 || areaToShow == 3)
                {
                    rot = Random.Range(0, 360);
                    current.transform.Rotate(current.transform.up, rot);
                    curNormal = current.transform.up;
                    if(areaToShow == 3)
                    {
                        curNormal *= -1f;
                    }
                    current.transform.rotation = Quaternion.FromToRotation(curNormal, info.normal) * current.transform.rotation;
                    current.transform.position = curPos;
                    Vector3 extents = currentInfo.body.bounds.extents;
                    extents = Vector3.Scale(extents, currentInfo.filter.transform.localScale);
                    Debug.Log(extents + "areaToShow"+ areaToShow);
                    Debug.Log("current.transform.position: " + current.transform.position + " before");
                    if (areaToShow == 1)
                    {
                        current.transform.Translate(0f, -extents.y + 0.8f, 0f, Space.Self);
                    }
                    else if (areaToShow == 3)
                    {
                        current.transform.Translate(0f, extents.y - 0.8f, 0f, Space.Self);
                    }
                    Debug.Log("current.transform.position: " + current.transform.position+ " after");
                }
                break;
            case TRASHAREA_FORM.CUBE:

                break;
        }
    }
}
