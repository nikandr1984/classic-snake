using UnityEngine;

public class ClearPlayerPrefs : MonoBehaviour
{
    [ContextMenu("Clear ALL Player Prefs")]
    public void ClearAll()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("ClearPlayerPrefs: All Player Prefs have been cleared.");
    }
}
