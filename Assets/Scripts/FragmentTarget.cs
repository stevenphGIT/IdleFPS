using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentTarget : MonoBehaviour
{
    private float explosionForce = 0.5f;
    private float fragmentLifetime = 0.2f;

    private void OnDestroy()
    {
        if(Options.Instance.targetExplode)
            SplitAndExplode();
    }

    private void SplitAndExplode()
    {
        Sprite originalSprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = originalSprite.texture;
        Vector2 spriteSize = originalSprite.bounds.size;

        float halfWidth = spriteSize.x / 2;
        float halfHeight = spriteSize.y / 2;

        Rect[] rects = new Rect[]
        {
            new Rect(0, 0.5f, 0.5f, 0.5f),
            new Rect(0.5f, 0.5f, 0.5f, 0.5f),
            new Rect(0, 0, 0.5f, 0.5f),
            new Rect(0.5f, 0, 0.5f, 0.5f)
        };

        Vector2[] positions = new Vector2[]
        {
            new Vector2(-halfWidth / 2, halfHeight / 2),
            new Vector2(halfWidth / 2, halfHeight / 2),
            new Vector2(-halfWidth / 2, -halfHeight / 2),
            new Vector2(halfWidth / 2, -halfHeight / 2)
        };

        for (int i = 0; i < 4; i++)
        {
            CreateFragment(texture, rects[i], positions[i]);
        }
    }

    private void CreateFragment(Texture2D texture, Rect uvRect, Vector2 localPosition)
    {
        GameObject fragment = new GameObject("Fragment");
        fragment.transform.position = transform.position + (Vector3)localPosition;

        fragment.transform.localScale = Vector3.one * 4f;

        Sprite fragmentSprite = Sprite.Create(
            texture,
            new Rect(
                uvRect.x * texture.width,
                uvRect.y * texture.height,
                uvRect.width * texture.width,
                uvRect.height * texture.height
            ),
            new Vector2(0.5f, 0.5f)
        );

        SpriteRenderer renderer = fragment.AddComponent<SpriteRenderer>();
        renderer.sprite = fragmentSprite;

        Rigidbody2D rb = fragment.AddComponent<Rigidbody2D>();
        rb.gravityScale = 2.5f;

        Vector2 explosionDir = localPosition.normalized;
        float rand1 = Random.Range(-0.3f, 0.3f);
        float rand2 = Random.Range(-0.3f, 0.3f);
        explosionDir += new Vector2(rand1, rand2);
        explosionForce = Random.Range(3f, 10f);
        rb.AddForce(explosionDir * explosionForce, ForceMode2D.Impulse);

        float randomTorque = Random.Range(-50f, -10f);
        rb.AddTorque(randomTorque, ForceMode2D.Impulse);

        fragment.AddComponent<FadeOut>();

        Destroy(fragment, fragmentLifetime);
    }
    private IEnumerator FadeOutFragment(SpriteRenderer renderer, float fadeDuration)
    {
        float startAlpha = renderer.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            Color newColor = renderer.color;
            newColor.a = alpha;
            renderer.color = newColor;
            yield return null;
        }
        Color finalColor = renderer.color;
        finalColor.a = 0f;
        renderer.color = finalColor;
    }
}
