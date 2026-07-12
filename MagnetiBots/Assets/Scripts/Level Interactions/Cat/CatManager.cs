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

        public void ChangeCat(Cat cat, int indexIncrease)
        {
            int highestIndex = catList.IndexOf(cat);
            if (highestIndex < catList.Count - 1)
            {
                for (int i = highestIndex + 1; i < catList.Count; i++)
                {
                    if (catList[i].gameObject.activeSelf)
                    {
                        highestIndex = i;
                    }
                }
            }
            catList[highestIndex + 1].gameObject.SetActive(true);
        }
    }
}
