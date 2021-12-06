
public class Skill {
    private string skillName;
    private int skillLevel;
    private int learnLevel;
    private string description;
    private bool isActive;
    private float coolTime;
    public Skill()
    {

    }

    public Skill(string skillName, int skillLevel, int learnLevel, string description, bool isActive,float coolTime)
    {
        this.SkillName = skillName;
        this.SkillLevel = skillLevel;
        this.LearnLevel = learnLevel;
        this.Description = description;
        this.IsActive = isActive;
        this.CoolTime = coolTime;
    }

    public string SkillName { get => skillName; set => skillName = value; }
    public int SkillLevel { get => skillLevel; set => skillLevel = value; }
    public int LearnLevel { get => learnLevel; set => learnLevel = value; }
    public string Description { get => description; set => description = value; }
    public bool IsActive { get => isActive; set => isActive = value; }
    public float CoolTime { get => coolTime; set => coolTime = value; }
}

