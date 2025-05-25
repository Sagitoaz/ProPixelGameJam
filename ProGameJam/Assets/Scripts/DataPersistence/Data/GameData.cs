using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int currentHealth;
    public int maxHealth;
    public int currentMana;
    public int maxMana;
    public int coin;
    public bool canDoubleJump;
    public bool canDash;
    public bool canSwim;
    public bool isFearBossDeath;
    public bool isSadBossDeath;
    public bool isAngryBossDeath;
    public bool isHopeBossDeath;
    public int totalYes;
    public int totalNo;
    public float musicVolume;
    public float sfxVolume;
    public Vector3 playerPosition;

    public InventorySlotData[] inventorySlots; // Lưu trạng thái của từng slot

    public GameData()
    {
        this.currentHealth = 3;
        this.maxHealth = 3;
        this.currentMana = 100;
        this.maxMana = 100;
        this.coin = 0;
        this.canSwim = false;
        this.canDoubleJump = false;
        this.canDash = false;
        this.isFearBossDeath = false;
        this.isSadBossDeath = false;
        this.isAngryBossDeath = false;
        this.isHopeBossDeath = false;
        this.totalYes = 0;
        this.totalNo = 0;
        this.musicVolume = 1f;
        this.sfxVolume = 1f;
        this.playerPosition = new Vector3(-57f, -25f, 0f);

        this.inventorySlots = new InventorySlotData[5];
        for (int i = 0; i < this.inventorySlots.Length; i++)
        {
            this.inventorySlots[i] = new InventorySlotData();
        }
    }
}

[System.Serializable]
public class InventorySlotData
{
    public int itemID; // ID của vật phẩm
    public int count;  // Số lượng vật phẩm trong slot

    public InventorySlotData()
    {
        this.itemID = -1; // -1 nghĩa là không có vật phẩm
        this.count = 0;
    }
}