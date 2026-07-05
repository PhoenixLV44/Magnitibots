using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SourcesList.asset", menuName = "SourcesListAsset")]
public class SourcesList : ScriptableObject
{
    public List<string> sources;
}
