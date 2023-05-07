using UnityEngine;

[System.Serializable][CreateAssetMenu(fileName="New Level Grade", menuName="Levels/LevelGrade")]
public class LevelGrades : ScriptableObject
{
    public Grades[] grades;
}

[System.Serializable]
public struct Grades
{
    public int scoreNeeded;
    public Sprite sprite;
}
