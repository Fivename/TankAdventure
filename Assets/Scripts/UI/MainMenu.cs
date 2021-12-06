using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //private static MainMenu _instance;
    //public static MainMenu Instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //            _instance = GameObject.Find("MainMenu").GetComponent<MainMenu>();
    //        return _instance;
    //    }
    //}

    private Text text;
    public Player player = Player.getInstance();
    public static bool isNewGame = false;
    public GameObject saveMenu;
    public GameObject mapManager;
    private SaveMenu saveMenuScript;
    //private Save showSave;
    public void NewGame()
    {
        //bool flag = GUI.Button(new Rect(0f, 0f, 0f,0f), "确定要开始新游戏吗？");
        //if (flag)
        //{
        //}
        InitPlayer();
        SceneManager.LoadScene("Scene0");
    }
    private Save curSave;
    
    private void InitPlayer()
    {
        player.Damage = 1;
        player.DoubleJumpSpeed = 5;
        player.JumpSpeed = 7;
        player.RunSpeed = 5;
        player.Health = 40;
        player.Defens = 1;
        player.Level = 1;
        player.Experience = 0;
        Skill sk = new Skill();
        sk.SkillName = "Dash";
        sk.IsActive = true;
        sk.CoolTime = 4;
        sk.LearnLevel = 1;
        player.CurSkill = sk;
        player.AddSkill(sk);
    }

    private void Awake()
    {
        saveMenuScript = saveMenu.GetComponent<SaveMenu>();
    }
    private void Start()
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
        curSave = JsonMapper.ToObject<Save>(playerText);
        player = curSave.JsonToPlayer(player);
        string s = string.Format("当前存档时间：{0}\n当前到达关卡:{1}\n当前角色属性：\n<等级>{2}\n", curSave.date, curSave.stage, curSave.level);
        text = transform.GetChild(5).GetChild(1).GetComponent<Text>();
        text.text = s;
    }

    private void Update()
    {
        if (saveMenu.activeSelf)
        {
            curSave = saveMenuScript.curSave;
            string s = string.Format("当前存档时间：{0}\n当前到达关卡:{1}\n当前角色属性：\n<等级>{2}\n", curSave.date, curSave.stage, curSave.level);
            text.text = s;
        }
    }


    public void SaveGame()
    {
        //InitPlayer();
        //Save save = new Save(player);
        //string json = JsonMapper.ToJson(save);
        //string savePath = Application.dataPath + @"/Save/NewSaveData.json";
        //File.WriteAllText(savePath, json, Encoding.UTF8);
        //SaveMenu.Instance.gameObject.SetActive(true);
        //SaveMenu.Instance.gameObject.GetComponent<SaveMenu>().mode = "Save";
        UIChange.ChangeUI(gameObject, saveMenu);
        //saveMenu.gameObject.SetActive(true);
        saveMenu.gameObject.GetComponent<SaveMenu>().mode = "Save";
        //gameObject.SetActive(false);
    }
    public void LoadGame()
    {
        //string savePath = Application.dataPath + @"/Save/NewSaveData.json";
        //Save save = new Save();

        //string playerText = File.ReadAllText(savePath);
        //save = JsonMapper.ToObject<Save>(playerText);
        //player = save.JsonToPlayer(player);

        //SceneManager.LoadScene("Scene0");
        //SaveMenu.Instance.gameObject.SetActive(true);
        //SaveMenu.Instance.gameObject.GetComponent<SaveMenu>().mode = "Load";
        UIChange.ChangeUI(gameObject, saveMenu);
        //saveMenu.gameObject.SetActive(true);
        saveMenu.gameObject.GetComponent<SaveMenu>().mode = "Load";
        //gameObject.SetActive(false);
    }

    public void ChoseMap()
    {
        UIChange.ChangeUI(gameObject, mapManager);
        //mapManager.gameObject.SetActive(true);
        //gameObject.SetActive(false);
    }

    public void SetSeting()
    {

    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
