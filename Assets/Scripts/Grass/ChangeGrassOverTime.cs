using UnityEngine;

public class ChangeGrassOverTime : MonoBehaviour
{

    [Header("Stage1 Grass")]
    [Header("Woods")]
    [SerializeField] private GrassStaging woodsStages;

    [Header("Path")]
    [SerializeField] private GrassStaging pathStages;

    [Header("Leaves")]
    [SerializeField] private GrassStaging leavesStages;


    public void UpdateGrass_Stage1_StartDecay() {
            woodsStages.UpdateGrass_StartDecay();
            pathStages.UpdateGrass_StartDecay();
            leavesStages.UpdateGrass_StartDecay();
    }
    public void UpdateGrass_Stage1_DecayMain() {
            woodsStages.UpdateGrass_DecayMain();
            pathStages.UpdateGrass_DecayMain();
            leavesStages.UpdateGrass_DecayMain();
        }
    public void UpdateGrass_Stage1_TrashRising() {
            woodsStages.UpdateGrass_TrashRising();
            pathStages.UpdateGrass_TrashRising();
            leavesStages.UpdateGrass_TrashRising();
        }
    public void UpdateGrass_Stage1_Final() {
            woodsStages.UpdateGrass_Final();
            pathStages.UpdateGrass_Final();
            leavesStages.UpdateGrass_Final();
        }
}
