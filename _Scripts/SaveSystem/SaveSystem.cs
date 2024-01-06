using System;
using System.Collections.Generic;
using CI.QuickSave;

public class SaveSystem : MonoBehaviour
{
    public enum SaveType
    {
        NewSave,
        Pass
    }

    /// <summary>
    ///     快速保存进度
    /// </summary>
    /// <param name="saveName">存档名</param>
    /// <param name="pass">关卡</param>
    public static void Save(string saveName, float pass)
    {
        //Todo: Save
        QuickSaveWriter.Create(saveName)
            .Write(SaveType.Pass.ToString(), pass)
            .Commit();
    }

    /// <summary>
    ///     保存单个数据，支持添加新的数据类型
    /// </summary>
    public static void SaveData<T>(string saveName, SaveType key, T value)
    {
        QuickSaveWriter.Create(saveName)
            .Write(key.ToString(), value)
            .Commit();
    }

    /// <summary>
    ///     快速加载存档
    /// </summary>
    /// <param name="saveName">存档名</param>
    /// <returns></returns>
    public static List<float> Load(string saveName)
    {
        var save = QuickSaveReader.Create(saveName);
        if (save == null) return null;

        var pass = new List<float>
        {
            save.Read<float>(SaveType.Pass.ToString())
        };

        return pass;
    }

    /// <summary>
    ///     检查存档是否存在
    /// </summary>
    /// <param name="saveName">存档名</param>
    /// <returns></returns>
    public static bool CheckSaveIsExist(string saveName)
    {
        return QuickSaveWriter.Create(saveName) != null;
    }

    /// <summary>
    ///     删除旧存档并且新建新存档
    /// </summary>
    /// <param name="saveName">存档名</param>
    /// <returns></returns>
    public static bool NewSave(string saveName)
    {
        try
        {
            QuickSaveRaw.Delete(saveName);

            const bool newSave = true;
            QuickSaveWriter.Create(saveName)
                .Write(SaveType.NewSave.ToString(), newSave)
                .Commit();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
    }
}