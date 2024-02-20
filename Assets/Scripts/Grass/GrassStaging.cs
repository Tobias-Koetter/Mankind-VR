using UnityEngine;

public class GrassStaging : MonoBehaviour
{
    [System.Serializable]
    public struct ColorsForStage
    {
        public Color topTint;
        public Color bottomTint;
    }
    [SerializeField] private GrassComputeScript currentGrass;
    [Header("Initial Colors")]
    public Color topTint_N;
    public Color bottomTint_N;
    [Header("Changing Properties")]
    public bool doStartDecay = false;
    [ConditionalHide("doStartDecay", true, false)] public GrassComputeScript switchModel_SD;
    [ConditionalHide("doStartDecay", true, false)] public Color topTint_SD;
    [ConditionalHide("doStartDecay", true, false)] public Color bottomTint_SD;
    [ConditionalHide("doStartDecay", true, false)] public float transitionTime_SD = 1f;
    public bool doDecayMain = false;
    [ConditionalHide("doDecayMain", true, false)] public GrassComputeScript switchModel_DM;
    [ConditionalHide("doDecayMain", true, false)] public Color topTint_DM;
    [ConditionalHide("doDecayMain", true, false)] public Color bottomTint_DM;
    [ConditionalHide("doDecayMain", true, false)] public float transitionTime_DM = 1f;
    public bool doTrashRising = false;
    [ConditionalHide("doTrashRising", true, false)] public GrassComputeScript switchModel_TR;
    [ConditionalHide("doTrashRising", true, false)] public Color topTint_TR;
    [ConditionalHide("doTrashRising", true, false)] public Color bottomTint_TR;
    [ConditionalHide("doTrashRising", true, false)] public float transitionTime_TR = 1f;
    public bool doFinal = false;
    [ConditionalHide("doFinal", true, false)] public GrassComputeScript switchModel_F;
    [ConditionalHide("doFinal", true, false)] public Color topTint_F;
    [ConditionalHide("doFinal", true, false)] public Color bottomTint_F;
    [ConditionalHide("doFinal", true, false)] public float transitionTime_F = 1f;

    private bool inColorChange = false;
    private ColorsForStage startColors;
    private ColorsForStage endColors;
    private float colorValue = 0f;
    private float total = 0f;
    private Color c;

    private void Update() {
        if (inColorChange)
        {
            colorValue += Time.deltaTime;
            colorValue = Mathf.Min(colorValue, total);

            c = Color.Lerp(startColors.topTint, endColors.topTint, colorValue / total);
            currentGrass.GetInstantiatedMaterial().SetColor("_TopTint", c);
            c = Color.Lerp(startColors.bottomTint, endColors.bottomTint, colorValue / total);
            currentGrass.GetInstantiatedMaterial().SetColor("_BottomTint", c);

            if (colorValue == total)
            {
                currentGrass.GetInstantiatedMaterial().SetColor("_TopTint", endColors.topTint);
                currentGrass.GetInstantiatedMaterial().SetColor("_BottomTint", endColors.bottomTint);

                // Reset
                colorValue = 0f;
                total = 0f;
                inColorChange = false;
            }
        }
    }
    public void UpdateGrass_StartDecay() {
        UpdateGrass(doStartDecay,transitionTime_SD, topTint_SD, bottomTint_SD, switchModel_SD);
    }
    public void UpdateGrass_DecayMain() {
        UpdateGrass(doDecayMain,transitionTime_DM, topTint_DM, bottomTint_DM, switchModel_DM);
    }
    public void UpdateGrass_TrashRising() {
        UpdateGrass(doTrashRising,transitionTime_TR, topTint_TR, bottomTint_TR, switchModel_TR);
    }
    public void UpdateGrass_Final() {
        UpdateGrass(doFinal,transitionTime_F, topTint_F, bottomTint_F, switchModel_F);
    }

    public void UpdateGrass(bool isActive,float totalTime, Color topTint, Color bottomTint, GrassComputeScript currentModel = null) {
        if (isActive)
        {
            if (currentModel != null)
            {
                // Switch material over Time
                currentGrass.gameObject.SetActive(false);
                currentGrass = currentModel;
                currentGrass.gameObject.SetActive(true);
            }
            else
            {
                // Change Color over Time
                inColorChange = true;
                colorValue = 0f;
                total = totalTime;

                endColors.topTint = topTint;
                endColors.bottomTint = bottomTint;
                startColors.topTint = currentGrass.GetInstantiatedMaterial().GetColor("_TopTint");
                startColors.bottomTint = currentGrass.GetInstantiatedMaterial().GetColor("_BottomTint");

            }
        }
    }
}
