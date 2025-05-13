using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : MonoBehaviour
{
    [SerializeField] private GameObject _miniMap;
    [SerializeField] private GameObject _largeMap;
    public bool IsLargeMap { get; private set; }
    public static MapManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        CloseLargeMap();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (!IsLargeMap)
            {
                OpenLargeMap();
            }
            else
            {
                CloseLargeMap();
            }
        }
    }

    public void OpenLargeMap()
    {
        _miniMap.SetActive(false);
        _largeMap.SetActive(true);
        IsLargeMap = true;
    }

    public void CloseLargeMap()
    {
        _miniMap.SetActive(true);
        _largeMap.SetActive(false);
        IsLargeMap = false;
    }
}