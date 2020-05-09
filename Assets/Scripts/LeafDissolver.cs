using System.Collections;
using UnityEngine;

public class LeafDissolver : MonoBehaviour
{
    public Material leaves;
    public bool isDissolving = false;

    // Update is called once per frame
    void Update()
    {
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
        
    }

    public void startDissovle()
    {
        isDissolving = true;
        StartCoroutine(HandleDissolve(true));
    }

    IEnumerator HandleDissolve(bool dir)
    {
        if (dir)
        {
            for (float val = 0f; val < 1.0f; val += 0.004f)
            {
                if (val > 1f || val+0.01f >= 1f)
                    val = 1f;
                Debug.Log(val);
                leaves.SetFloat("Vector1_2E022231", val);
                yield return new WaitForSeconds(0.01f);
            }
        }
        else if (!dir)
        {
            for (float val = 1f; val > 0f; val -= 0.004f)
            {
                if(val < 0f || val - 0.01f <= 0f)
                    val = 0f;
                Debug.Log(val);
                leaves.SetFloat("Vector1_2E022231", val);
                yield return new WaitForSeconds(0.01f);
            }
        }
        isDissolving = false;
        yield return null;
    }
}
