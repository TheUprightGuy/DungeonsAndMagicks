using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [Header("Required Fields")]
    public Mesh openMesh;
    public Mesh closeMesh;
    public GameObject goldPrefab;
    public Transform spawnPoint;
    [Header("Loot Fields")]
    public List<GameObject> loot;
    [Range(0, 1000)]
    public int gold;

    // Local Variables
    bool open = false;
    Player pc;
    MeshFilter meshFilter;

    #region Setup
    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = closeMesh;
    }
    #endregion Setup

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && pc && !open)
        {
            open = true;
            meshFilter.mesh = openMesh;

            StartCoroutine(CreateLoot());
        }
    }

    IEnumerator CreateLoot()
    {
        // Add in Animation Delay Here
        yield return new WaitForSeconds(0.5f);

        // Randomize Gold Piles
        int minimum = (int)gold / 10;
        int maximum = (int)gold / 4;
        // Gold
        for (int i = 0; i < 10; i++)
        {
            // Randomizing pile amount
            int goldAmount = Random.Range(Mathf.Min(minimum, gold), Mathf.Min(gold, maximum));
            // Instantiate Loot & Add Loot Explosion Effect
            Gold temp = Instantiate(goldPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Gold>();
            temp.amount = goldAmount;
            temp.gameObject.AddComponent<LootEffect>();

            gold -= goldAmount;
            // All Gold Given
            if (gold <= 0)
            {
                break;
            }
            yield return new WaitForSeconds(0.15f);
        }

        // Loot
        for (int i = 0; i < loot.Count; i++)
        {
            // Instantiate Loot & Add Loot Explosion Effect
            GameObject temp = Instantiate(loot[i], spawnPoint.position, Quaternion.identity);
            temp.AddComponent<LootEffect>();
            yield return new WaitForSeconds(0.15f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            pc = other.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            pc = null;
        }
    }
}
