using System;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager Instance;
    public int choosenSkinID;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    public void SetSkinID(int skinID) => choosenSkinID = skinID;
    public int GetSkinID() => choosenSkinID;
}
