using UnityEngine;

public class BrokenTreePart: MonoBehaviour
{
    [SerializeField]
    private int partID;
    [SerializeField]
    private string partName;
    [SerializeField]
    private bool isAlive = true;
    public LeafDissolver lDis;
    public BrokenTreePart childToDestroyFirst;

    private MeshRenderer ownRenderer;
    private MeshCollider ownCollider;

    public BrokenTreePart(int id, string name)
    {
        partID = id;
        this.partName = name;
    }

    void Start()
    {
        ownCollider = gameObject.GetComponent<MeshCollider>();
        ownRenderer = gameObject.GetComponent<MeshRenderer>();
        lDis = gameObject?.GetComponent<LeafDissolver>();
        if(lDis)
        {
            lDis.leaves = ownRenderer.materials;
        }
    }

    public int GetPartID => partID;
    public string GetPartName => partName;
    public void SetAliveState(bool alive)
    { 
        isAlive = alive;
        ownRenderer.enabled = alive;
        ownCollider.enabled = alive;
    }
    public bool Alive => isAlive;

}
