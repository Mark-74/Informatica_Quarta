using UnityEngine;

public class spriteManager : MonoBehaviour
{
    public static spriteManager instance;
    public Sprite[] numberSprites;
    public Sprite flagSprite;
    public Sprite mineSprite;
    public Sprite closedSprite;

    private void Awake()
    {
        instance = this;
    }
}
