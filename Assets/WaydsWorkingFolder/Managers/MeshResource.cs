using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MeshObject
{
    public string meshName;
    public GameObject meshObject;
}

[CreateAssetMenu(fileName = "MeshResource", menuName = "Data/MeshResource", order = 1)]

public class MeshResource : ScriptableObject
{
    public List<MeshObject> objects;

    public GameObject GetMesh(ItemType _type)
    {
        string _meshName = _type.ToString();

        foreach(MeshObject n in objects)
        {
            if (n.meshName == _meshName)
            {
                return (n.meshObject);
            }
        }

        return null;
    }
}
