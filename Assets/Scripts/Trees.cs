using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
public enum TREE_STAGE { ALIVE1, ALIVE2, BETWEEN1, BETWEEN2, DEAD };

public class Trees : Interactable
{
    //public Transform alive;
    //public GameObject[] destroyed;

    public TREE_STAGE status = TREE_STAGE.ALIVE1;
    public bool isDestroyed = false;

    private GameObject stage1;
    private GameObject stage1_2;
    private GameObject stage2;
    private GameObject stage2_2;
    private GameObject stage3;

    public List<Transform> models1;
    public List<Transform> models1_2;
    public List<Transform> models2;
    public List<Transform> models2_2;
    public List<Transform> models3;

    private Transform currentModel;
    private DestroyVegetation controller;
    private int treeNumber;

    public DestroyVegetation Controller { get => controller; set => controller = value; }
    public int TreeNumber { get => treeNumber; set => treeNumber = value; }

    private void Start()
    {
        InitFillLists();
        List<Transform> curList = FindCurrentList();
        currentModel = GetModel(curList);
        currentModel.gameObject.SetActive(true);


        /*foreach (GameObject g in destroyed)
            g.SetActive(false);*/
    }


    public override void Interact()
    {
        status += 1;
        isDestroyed = true;

        currentModel.gameObject.SetActive(false);
        currentModel = GetModel(FindCurrentList());
        currentModel.gameObject.SetActive(true);
    }

    public override string ToString()
    {

        return "| Tree [" + treeNumber + "] " + (isDestroyed ? "isDead" : "isAlive");
    }

    private void Update()
    {
        /*
        if (isDestroyed)
        {
            alive.gameObject.SetActive(false);
            int index = Random.Range(0, destroyed.Length);
            destroyed[index].SetActive(true);
        }
        */
    }

    private void InitFillLists()
    {
        foreach (Transform t in transform)
        {
            //Debug.Log(t);
            switch (t.name)
            {
                case "Stage1":
                    if (!stage1) { stage1 = t.gameObject; }
                    break;
                case "Stage1_2":
                    if (!stage1_2) { stage1_2 = t.gameObject; }
                    break;
                case "Stage2":
                    if (!stage2) { stage2 = t.gameObject; }
                    break;
                case "Stage2_2":
                    if (!stage2_2) { stage2_2 = t.gameObject; }
                    break;
                case "Stage3":
                    if (!stage3) { stage3 = t.gameObject; }
                    break;
            }

        }

        string pattern = @"\b[prefab]\w+";
        Regex rg = new Regex(pattern);
        foreach (Transform t in stage1.transform)
        {
            if (rg.IsMatch(t.name))
            {
                models1.Add(t);
                t.gameObject.SetActive(false);
            }
        }
        foreach (Transform t in stage1_2.transform)
        {
            if (rg.IsMatch(t.name))
            {
                models1_2.Add(t);
                t.gameObject.SetActive(false);
            }
        }
        foreach (Transform t in stage2.transform)
        {
            if (rg.IsMatch(t.name))
            {
                models2.Add(t);
                t.gameObject.SetActive(false);
            }
        }
        foreach (Transform t in stage2_2.transform)
        {
            if (rg.IsMatch(t.name))
            {
                models2_2.Add(t);
                t.gameObject.SetActive(false);
            }
        }
        foreach (Transform t in stage3.transform)
        {
            if (rg.IsMatch(t.name))
            { 
                models3.Add(t);
                t.gameObject.SetActive(false);
            }
        }
    }

    private List<Transform> FindCurrentList()
    {
        switch(status)
        {
            case TREE_STAGE.ALIVE1:
                return models1;
            case TREE_STAGE.ALIVE2:
                return models1_2;
            case TREE_STAGE.BETWEEN1:
                return models2;
            case TREE_STAGE.BETWEEN2:
                return models2_2;
            case TREE_STAGE.DEAD:
                return models3;
        }
        return null;
    }

    private Transform GetModel(List<Transform> tList)
    {
        int l = tList.Count;
        if(l == 0)
        {
            Debug.LogError($"The Model Array {tList} should not be empty.");
            return null;
        }
        else if(l == 1)
        {
            return tList[0];
        }
        else
        {
            int index = Random.Range(0, l);

            return tList[index];
        }
    }
}
