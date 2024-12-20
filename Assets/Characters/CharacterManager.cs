using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    // Singleton instance
    public static CharacterManager Instance { get; private set; }

    // Karakter prefab'larýnýn tutulduðu liste
    public List<GameObject> characterPrefabs;

    private void Awake()
    {
        // Singleton örneðini oluþtur
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne deðiþse bile yok olmasýn
        }
        else
        {
            Destroy(gameObject); // Baþka bir instance varsa yok et
        }
    }

    // Listedeki karakterlerden rastgele birini döndürür
    public GameObject GetRandomCharacter()
    {
        if (characterPrefabs != null && characterPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, characterPrefabs.Count);
            return characterPrefabs[randomIndex];
        }
        else
        {
            Debug.LogError("Character Prefabs listesi boþ veya tanýmlanmamýþ!");
            return null;
        }
    }
}
