using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class entry : MonoBehaviour
{
    public TMP_Text entryText;
    public Image entryImage;
    public Image sourceRED;
    public Image sourceGREEN;

    public void SetEntry(string text, Sprite image, Sprite redSource, Sprite greenSource)
    {
        entryText.text = text;
        entryImage.sprite = image;
        sourceRED.sprite = redSource;
        sourceGREEN.sprite = greenSource;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
