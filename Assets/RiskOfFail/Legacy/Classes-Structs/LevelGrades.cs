using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "New Level Grade", menuName = "Levels/LevelGrade")]
public class LevelGrades : ScriptableObject
{
    public Grades[] grades;
}

[Serializable]
public struct Grades
{
    public int scoreNeeded;
    public Sprite sprite;
}