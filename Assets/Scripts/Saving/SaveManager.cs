using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string _buildingsPath = Path.Combine(Application.persistentDataPath, "_buildings.json");
    private static string _walletPath = Path.Combine(Application.persistentDataPath, "_wallet.json");

    [System.Serializable]
    public class BuildingSaveData
    {
        public List<BuildingData> _buildings = new List<BuildingData>();
    }

    [System.Serializable]
    private class WalletSaveData
    {
        public int balance;
    }

    // --- Buildings ---
    public static void SaveBuildings(List<BuildingData> buildingsData)
    {
        string json = JsonUtility.ToJson(new BuildingSaveData { _buildings = buildingsData }, true);
        File.WriteAllText(_buildingsPath, json);
    }

    public static List<BuildingData> LoadBuildings()
    {
        if (!File.Exists(_buildingsPath)) return new List<BuildingData>();
        string json = File.ReadAllText(_buildingsPath);
        return JsonUtility.FromJson<BuildingSaveData>(json)._buildings;
    }

    // --- Wallet ---
    public static void SaveWalletBalance(int balance)
    {
        string json = JsonUtility.ToJson(new WalletSaveData { balance = balance }, true);
        File.WriteAllText(_walletPath, json);
    }

    public static int LoadWalletBalance()
    {
        if (!File.Exists(_walletPath)) return 0;
        string json = File.ReadAllText(_walletPath);
        return JsonUtility.FromJson<WalletSaveData>(json).balance;
    }
}
