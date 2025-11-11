using UnityEngine;

[System.Serializable] 
public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public float gameTime;    


    public LeaderboardEntry(string name, int s, float time = 0f) // Конструктор с параметрами
    {
        playerName = name;
        score = s;
        gameTime = time;        
    }

    public string GetFormattedTime()
    {
        int minutes = Mathf.FloorToInt(gameTime / 60F);
        int seconds = Mathf.FloorToInt(gameTime % 60);
        return $"{minutes:D2}:{seconds:D2}";
    }
}
