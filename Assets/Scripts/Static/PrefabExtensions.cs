using UnityEngine;

public static class PrefabExtensions
{
    public static string GetPrefabName(this GameObject self)
    {
        return self.name.Split()[0];
    }

    public static void AddPrefabSurname(this GameObject self, string surname)
    {
        self.name += $" {surname}";
    }
}
