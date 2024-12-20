using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    // Singleton instance
    public static CharacterManager Instance { get; private set; }

    // Karakter prefab'lar�n�n tutuldu�u liste
    public List<GameObject> characterPrefabs;

    private void Awake()
    {
        // Singleton �rne�ini olu�tur
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Sahne de�i�se bile yok olmas�n
        }
        else
        {
            Destroy(gameObject); // Ba�ka bir instance varsa yok et
        }
    }

    // Listedeki karakterlerden rastgele birini d�nd�r�r
    public GameObject GetRandomCharacter()
    {
        if (characterPrefabs != null && characterPrefabs.Count > 0)
        {
            int randomIndex = Random.Range(0, characterPrefabs.Count);
            return characterPrefabs[randomIndex];
        }
        else
        {
            Debug.LogError("Character Prefabs listesi bo� veya tan�mlanmam��!");
            return null;
        }
    }
}
