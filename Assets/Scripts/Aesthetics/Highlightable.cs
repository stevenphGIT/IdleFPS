using UnityEngine;
using UnityEngine.UI;

public class Highlightable : MonoBehaviour
{
    protected Material og;
    private Material mat;
    public bool showDuringDialogue = false;
    public bool useImage = false;
    private void Awake()
    {
        if (useImage)
            og = this.gameObject.GetComponent<Image>().material;
        else
            og = this.gameObject.GetComponent<SpriteRenderer>().material;

        mat = new Material(og);

        if (useImage)
            this.gameObject.GetComponent<Image>().material = mat;
        else
            this.gameObject.GetComponent<SpriteRenderer>().material = mat;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!showDuringDialogue && CutsceneHandler.Instance.inCutscene) return;
        if (!showDuringDialogue && SpeechBox.Instance.boxShowing) return;
        mat.SetFloat("_OutlineEnabled", 1);
        mat.SetColor("_SolidOutline", Color.white);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        mat.SetFloat("_OutlineEnabled", 0);
    }
}
