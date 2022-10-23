using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Istaka : MonoBehaviour
{
    public static Istaka Instance; 
    [SerializeField] private List<GameObject> placeTransform = new List<GameObject>();
    private int i = 0;
    private void Awake()
    {
        Instance = this;
    }

    public void MakeCue(List<Place> stones)
    {
        
        foreach (Place stone in stones)
        {
            stone.unitHolder.gameObject.transform.parent = placeTransform[i].transform;
            stone.unitHolder.gameObject.transform.DOScale(new Vector3(0.1f, 0.02f, 0.33f), 1f);
            stone.unitHolder.gameObject.transform.DOMove(placeTransform[i].transform.position, 1f);
            stone.unitHolder.gameObject.transform.DORotate(new Vector3(35, 180, 0), 1f);
            stone.unitHolder = null;
            i++;
        }
    }

    public void Regulator(List<Place> stones)
    {
        for (int k = 0; k < 2; k++)
        {
            for (int j = 0; j < 2; j++)
            {
                if (Convert.ToDouble(stones[j].unitHolder.gameObject.tag)>Convert.ToDouble(stones[j+1].unitHolder.gameObject.tag))
                {
                    Place item = stones[j];
                    stones[j] = stones[j + 1];
                    stones[j + 1] = item;
                }
            }
        }
        MakeCue(stones);
    }
}
