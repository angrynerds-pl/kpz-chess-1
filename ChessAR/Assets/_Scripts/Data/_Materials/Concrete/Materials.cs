using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour, IMaterials
{
    [SerializeField] private List<MaterialElement> materialsList = null;

    public Material getByIndex(MaterialIndex index)
    {
        return materialsList.Find(i => i.Index == index).Material;
    }
}
