using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class SaveMenu : MonoBehaviour
{
    //private static SaveMenu _instance;
    //public static SaveMenu Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            _instance = GameObject.Find("SaveMenu").GetComponent<SaveMenu>();
    //        return _instance;
    //    }
    //}
    public GameObject mainMenu;
    Player player = Player.getInstance();
    public string mode="Save";
    public Save curSave;
    public Save lastSave;
    public Text showModeText;
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void OnEnable()
    {
        //一开始就要显示在界面的就是最后一次游玩时的数据
        string LastSaveJson = Application.dataPath + @"/Save/LastSaveData.json";
        //如果没有==第一次玩 数据就是初始数据
        if (!File.Exists(LastSaveJson))
        {
            LastSaveJson = Application.dataPath + @"/Save/NewSaveData.json";
        }
        string playerText = File.ReadAllText(LastSaveJson);
        //获取到最后一次游玩的存档作为当前存档
        curSave = new Save();
        lastSave = new Save();
        curSave = JsonMapper.ToObject<Save>(playerText);
        lastSave = JsonMapper.ToObject<Save>(playerText);
        player = curSave.JsonToPlayer(player);
        showModeText = gameObject.transform.GetChild(10).GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        showModeText.text = mode+"-Mode";

    }
    //当前存档要读取的数据
    public void LoadData(int id)
    {
        string loadJson = Application.dataPath + @"/Save/SaveData_" + id.ToString() + ".json";
        if (!File.Exists(loadJson))
        {
            Debug.LogError("There is a Empty Save！");
        }
        else
        {
            string playerText = File.ReadAllText(loadJson);
            curSave = JsonMapper.ToObject<Save>(playerText);
            curSave.id = id;
            player = curSave.JsonToPlayer(player);
        }
        BackTOMainMenu();
        

    }
    //当前存档数据要保存的位置
    public void SaveData(int id)
    {
        string saveJson = Application.dataPath + @"/Save/SaveData_" + id.ToString() + ".json";
        //获取当前数据 转换成json
        string json = JsonMapper.ToJson(curSave);
        //将json数据写入该id的存档
        File.WriteAllText(saveJson, json, Encoding.UTF8);


    }

    public void SetSaveMode()
    {
        showModeText.text = "Save-Mode";
        mode = "Save";
    }
    public void SetLoadMode()
    {
        showModeText.text = "Load-Mode";
        mode = "Load";
    }
    public void BackTOMainMenu()
    {
        //MainMenu.Instance.gameObject.SetActive(true);
        UIChange.ChangeUI(gameObject, mainMenu);
        //mainMenu.gameObject.SetActive(true);
        //gameObject.SetActive(false);
    }

}
