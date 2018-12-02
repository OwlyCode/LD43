using System.Collections;
using UnityEngine;

public class Fading : MonoBehaviour
{
    private float fadeSpeed;
    private Texture2D texture;

    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;

    public static IEnumerator In(Color color, float speed = 0.8f, float sleep = 0.5f)
    {
        Fading fading = Create(color, speed);

        yield return new WaitForSeconds(fading.FadeIn() + sleep);
        yield return new WaitForEndOfFrame();
        Destroy(fading);
    }

    public static IEnumerator Out(Color color, float speed = 0.8f, float sleep = 0.5f)
    {
        Fading fading = Create(color, speed);

        yield return new WaitForSeconds(fading.FadeOut() + sleep);
        yield return new WaitForEndOfFrame();
        Destroy(fading);
    }

    private static Fading Create(Color color, float speed = 0.8f)
    {
        GameObject go = new GameObject();

        go.AddComponent<Fading>();
        Fading fading = go.GetComponent<Fading>();

        fading.texture = new Texture2D(1, 1);
        fading.texture.SetPixel(0, 0, color);
        fading.texture.Apply();

        fading.fadeSpeed = speed;

        return fading;
    }

    private float FadeIn()
    {
        fadeDir = -1;
        alpha = 1f;

        return 1 / fadeSpeed;
    }

    private float FadeOut()
    {
        fadeDir = 1;
        alpha = 0f;

        return 1 / fadeSpeed;
    }

    void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);

        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), texture);
    }
}
