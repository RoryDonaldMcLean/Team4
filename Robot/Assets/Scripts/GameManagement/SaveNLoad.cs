using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveNLoad
{
    public static void Save(PlayerSetting commentData)
    {
        CreateFile(commentData);
    }

    private static void CreateFile(PlayerSetting commentData)
    {
        BinaryFormatter bf = new BinaryFormatter();
        if (!File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.setting"))
        {
            File.Create(Application.persistentDataPath + "/PlayerButtonSetting.setting").Close();
        }
        FileStream file = File.Open(Application.persistentDataPath + "/PlayerButtonSetting.setting", FileMode.Open);
        bf.Serialize(file, commentData);
        file.Close();
    }

    public static PlayerSetting Load()
    {
        PlayerSetting commentData = new PlayerSetting();
        //change this number when config is altered
        string versionInfo = "3.0";

        if (File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.setting"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerButtonSetting.setting", FileMode.Open);
            commentData = (PlayerSetting)bf.Deserialize(file);
            file.Close();

            //if not match then delete
            if (!commentData.versionInfo.Contains(versionInfo))
            {
                File.Delete(Application.persistentDataPath + "/PlayerButtonSetting.setting");
                commentData = null;

                GameManager.Instance.playerSetting.versionInfo = versionInfo;
                CreateFile(GameManager.Instance.playerSetting);
                commentData = Load();
            }
            if (commentData == null)
                Debug.LogError("NULL");

            return commentData;
        }
        return null;
    }
}
