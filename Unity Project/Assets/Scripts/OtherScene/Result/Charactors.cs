using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactors : MonoBehaviour
{
    [SerializeField]
    private List<Animator> charactorAnimatorList = new List<Animator>();
    public List<Animator> CharactorAnimatorList { get { return charactorAnimatorList; } set { charactorAnimatorList = value; } }
    private void Start()
    {
       var charactorsArray = GetComponentsInChildren<Transform>();
       for(int i = 1;i <= charactorsArray.Length -1;i++)
       {
            CharactorAnimatorList.Add(charactorsArray[i].GetComponent<Animator>());
       }
    }
}
