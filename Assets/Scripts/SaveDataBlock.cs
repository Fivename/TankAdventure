using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;
using System.IO;
using LitJson;

public class SaveDataBlock : MonoBehaviour, IPointerDownHandler
{
    private int id;
    private SaveMenu saveMenu;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        id = int.Parse(transform.GetChild(2).GetComponent<Text>().text.Replace("存档", ""));
        saveMenu = transform.parent.parent.gameObject.GetComponent<SaveMenu>();
        text = transform.GetChild(1).GetComponent<Text>();
    }

    //每次启用都读取一遍存档数据并在ui更新显示
    void OnEnable()
    {
        Invoke("OnEnableDelay", 0.02f);
    }
    void OnEnableDelay()
    {
        Save save;
        string loadJson = Application.dataPath + @"/Save/SaveData_" + id.ToString() + ".json";
        if (!File.Exists(loadJson))
        {
            string ShowSaveString = "无存档";
            text.text = ShowSaveString;
        }
        else
        {
            string playerText = File.ReadAllText(loadJson);
            save = JsonMapper.ToObject<Save>(playerText);
            string ShowSaveString = string.Format("Level:{0}级\nMoney:{1}金", save.level, save.money);
            text.text = ShowSaveString;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("点了！");

        if (saveMenu.mode == "Save")
        {
            //让savemenu存档这个id的数据
            transform.parent.parent.SendMessage("SaveData", id);
            //在UI更新数据显示
            UpdateSave();
            //saveMenu.BackTOMainMenu();
        }
        else if (saveMenu.mode == "Load")
        {
            //让savemenu读取这个存档的数据
            transform.parent.parent.SendMessage("LoadData", id);
            saveMenu.BackTOMainMenu();
        }
        else
        {
            Debug.LogError("Error SaveOrLoad mode!!");
        }
    }

    /// <summary>
    /// 更新存档UI的显示
    /// </summary>
    void UpdateSave()
    {
        string ShowSaveString = string.Format("Level:{0}级\nMoney:{1}金", saveMenu.curSave.level, saveMenu.curSave.money);
        text.text = ShowSaveString;
    }

}
