public class LevelCompleation : UnityEngine.MonoBehaviour
{
    public static event System.Action LevelCompleated;

    public static void InvokeLevelCompleated() => LevelCompleated?.Invoke();
}