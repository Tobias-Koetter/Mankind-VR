
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.ProBuilder;

public class LevelBalancing
{
    public static float BalancingValue = 1000f;

    private static float NatureValue;
    private static float TrashValue;
    //private static int count = 1;


    public LevelBalancing()
    {
        ResetBalanceValue(0f);
    }

    public static void ResetBalanceValue(float newBalance)
    {
        //Debug.LogWarning("ResetBalanceValue called:"+ count++);
        BalancingValue = newBalance;
        NatureValue = newBalance;
        TrashValue = 0f;
    }

    /**
     * Returns the Balance Factor of the game regarding Trashspawns and Nature in total.
     *  0 => game is balanced
     * >0 => game is unbalanced ||>> more Trash needs to be spawned
     * <0 => game is unbalanced ||>> Nature must be destroyed
    **/
    public static float GetBalanceVariance() => (NatureValue + TrashValue) - BalancingValue;
    public static float GetCurrentNatureValue() => NatureValue;
    public static float GetCurrentTrashValue() => TrashValue;

    public static void SetNatureValue(float valueDestroyed, bool normalUse = true) { _ = normalUse ? (NatureValue -= valueDestroyed) : (NatureValue += valueDestroyed); }
    public static void SetTrashValue(float addedTrashValue, bool normalUse = true) { _ = normalUse ? (TrashValue += addedTrashValue) : (TrashValue += addedTrashValue); }


}
