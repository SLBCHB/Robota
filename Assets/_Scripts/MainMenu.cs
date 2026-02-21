using UnityEngine;

public class MainMenu : MonoBehaviour
{
    
    public void Play()
    {
        SceneController.Instance.LoadScene(GameScene.Intro);
    }
}
