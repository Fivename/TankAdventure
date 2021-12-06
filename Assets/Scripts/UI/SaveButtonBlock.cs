using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class SaveButtonBlock : MonoBehaviour
{
    private int id;
    private SaveMenu saveMenu;
    private Text text;
    public delegate void DoSaveLoad(int id);
    DoSaveLoad save;
    DoSaveLoad load;
    //在a时间增加委托 在b时间再调用委托
    // Start is called before the first frame update
    void Start()
    {
        id = int.Parse(transform.GetChild(0).GetChild(2).GetComponent<Text>().text.Replace("存档", ""));
        saveMenu = transform.parent.gameObject.GetComponent<SaveMenu>();
        text = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        save = new DoSaveLoad(saveMenu.SaveData);
        load = new DoSaveLoad(saveMenu.LoadData);
    }

    public void Do()
    {
        if (saveMenu.mode == "Save")
        {
            //saveMenu.SaveData(id);
            //用委托的话
            save(id);
            //transform.parent.SendMessage("SaveData", id);
            UpdateSave();
            saveMenu.BackTOMainMenu();
        }
        else if (saveMenu.mode == "Load")
        {
            load(id);
            //transform.parent.SendMessage("LoadData", id);
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
        string ShowSaveString = string.Format("Level:{0}级\nMoney:{1}金",saveMenu.curSave.level,saveMenu.curSave.money);
        text.text = ShowSaveString;
    }

}
