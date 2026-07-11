using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cat
{
    public class CatManager : MonoBehaviour
    {
        [SerializeField] private List<Cat> catList;
        public List<Cat> CatList => catList;
        private Cat[] _catArray;

        private void Start()
        {
            _catArray = GetComponentsInChildren<Cat>();
            catList.AddRange(_catArray);
            for (int i = 0; i < _catArray.Length; i++)
            {
                catList[i]  = _catArray[i];
                if (i == 0)
                {
                    catList[i].gameObject.SetActive(true);
                }
                else
                {
                    catList[i].gameObject.SetActive(false);
                }
            }
        }

        public void ChangeCat(Cat cat)
        {
            int index = catList.IndexOf(cat);
            catList[index + 1].gameObject.SetActive(true);
            cat.Animator.Play("SitIdle");
        }
    }
}
