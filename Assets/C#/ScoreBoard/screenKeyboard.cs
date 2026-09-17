using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class screenKeyboard : MonoBehaviour
{
    [SerializeField] Transform spawnPos;
    [SerializeField] List<GameObject> AllKeys = new List<GameObject>();
    [SerializeField] GameObject KeyPrefab;
    [SerializeField] List<char> value = new List<char>();

    [SerializeField] int selectedKey;

    [SerializeField] string namn;

    void Start()
    {
        for (int i = 0; i < value.Count; i++)
        {
            GameObject spawned = Instantiate(KeyPrefab, spawnPos);
            spawned.GetComponentInChildren<TMP_Text>().text = value[i].ToString();
            AllKeys.Add(spawned);
        }
    }

    void OnMove(InputValue inputValue)
    {
        Vector2 dir = inputValue.Get<Vector2>();
        if(dir.y != 0)
        {
            selectedKey += (int)(value.Count/2 * dir.y);
        } 
        selectedKey += (int)dir.x;
    }

    void OnSelect()
    {
        namn = namn.Insert(namn.Length, value[selectedKey].ToString()); 
    }

    void OnRemove()
    {
        namn = namn.Remove(namn.Length); 
    }
}
