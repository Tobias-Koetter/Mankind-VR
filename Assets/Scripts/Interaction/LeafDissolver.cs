using System.Collections;
using UnityEngine;

public class LeafDissolver : MonoBehaviour
{
    public Material[] leaves;
    public bool isDissolving = false;
    public TreeInformation currentActive;
    private float value = 0f;
    private bool dir = true;

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Alpha1) && !isDissolving)
        {
            isDissolving = true;
            StartCoroutine(HandleDissolve(true));
        }
        if(Input.GetKeyDown(KeyCode.Alpha2) && !isDissolving)
        {
            isDissolving = true;
            StartCoroutine(HandleDissolve(false));
        }
        */
        if (isDissolving)
        {
            HandleDissolveUpdate(dir);
        }
    }

    public void startDissovle()
    {
        isDissolving = true;
        value = 0.05f;
        dir = true;
        //StartCoroutine(HandleDissolve(true));
    }

    IEnumerator HandleDissolve(bool dir)
    {
        if (dir)
        {
            for (float val = 0f; val < 1.0f; val += 0.002f)
            {
                if (val > 1f || val+0.01f >= 1f)
                    val = 1f;
                //Debug.Log(val);
                foreach (Material dissolving in leaves)
                {
                    dissolving.SetFloat("_dissolve", val);
                }
                yield return new WaitForSeconds(0.0025f);
            }
        }
        else if (!dir)
        {
            for (float val = 1f; val > 0f; val -= 0.002f)
            {
                if(val < 0f || val - 0.01f <= 0f)
                    val = 0f;
                //Debug.Log(val);
                foreach (Material dissolving in leaves)
                {
                    dissolving.SetFloat("_dissolve", val);
                }
                yield return new WaitForSeconds(0.0025f);
            }
        }
        isDissolving = false;
        yield return null;
    }

    bool HandleDissolveUpdate(bool dir)
    {
        if (dir)
        {
            if(currentActive != null && currentActive.CheckAnyRendererIsVisible(false))
            {
                value = 1f;
                isDissolving = false;
                return true;
            }
            value += 0.2f* Time.deltaTime;
            if (value > 1f || value + 0.01f >= 1f)
            {
                value = 1f;
                isDissolving = false;
                return true;
            }

            //Debug.Log(val);
            foreach (Material dissolving in leaves)
            {
                dissolving.SetFloat("_dissolve", value);
            }
        }
        else if (!dir)
        {
            if (currentActive != null && currentActive.CheckAnyRendererIsVisible(false))
            {
                value = 0f;
                isDissolving = false;
                return true;
            }
            value -= 0.2f * Time.deltaTime;
            if (value < 0f || value - 0.01f <= 0f)
            {
                value = 0f;
                isDissolving = false;
                return true;
            }
            //Debug.Log(val);
            foreach (Material dissolving in leaves)
            {
                dissolving.SetFloat("_dissolve", value);
            }
        }
        return false;
    }
}
