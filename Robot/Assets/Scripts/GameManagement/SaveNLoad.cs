using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveNLoad
{
    public static void Save(PlayerSetting commentData)
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
        if (File.Exists(Application.persistentDataPath + "/PlayerButtonSetting.setting"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/PlayerButtonSetting.setting", FileMode.Open);
            commentData = (PlayerSetting)bf.Deserialize(file);
            file.Close();
            return commentData;
        }
        return null;
    }
}
