using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSystem : MonoBehaviour
{
    public static SoundSystem Instance; // Singleton instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnPlayerMakesSound(Transform player)
    {
        Debug.Log("Sound detected! Broadcasting to monsters.");

        // find all monsters in the scene and notify them
        MonsterAI[] monsters = FindObjectsOfType<MonsterAI>();

        foreach (MonsterAI monster in monsters)
        {
            monster.startInvestigating(player);
        }
    }
}
