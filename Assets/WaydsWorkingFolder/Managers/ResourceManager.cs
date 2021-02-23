using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class ResourceManager : MonoBehaviour
{
    public MeshResource mesh;

    private static ResourceManager resourceManager;
    public static ResourceManager instance
    {
        get
        {
            if (!resourceManager)
            {
                resourceManager = FindObjectOfType(typeof(ResourceManager)) as ResourceManager;
                if (!resourceManager)
                {
                    Debug.LogError("Resource Manager is missing!");
                }
                else
                {
                    // Hmm
                }
            }
            return resourceManager;
        }
    }

    public GameObject GetItemMesh(ItemType _type)
    {
        return(mesh.GetMesh(_type));
    }
}
