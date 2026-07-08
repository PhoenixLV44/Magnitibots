using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "SourcesList.asset", menuName = "SourcesListAsset")]
public class SourcesList : ScriptableObject
{
    [System.Serializable]
    public struct Source
    {
        public string name;
        public string contribution;
    }

    public List<Source> sourceList;
}
