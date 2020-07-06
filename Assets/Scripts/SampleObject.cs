using UnityEngine;

/// <summary>
/// Näytteiden Scriptableobjekti joka nopeuttaa näytteiden teon
/// </summary>
[CreateAssetMenu(fileName = "Sample", menuName = "ScriptableObjects/Sample",order = 1)]
public class SampleObject : ScriptableObject
{
    public string SampleName;

    public Texture TextureOf10Mag;
    public Texture TextureOf20Mag;
    public Texture TextureOf60Mag;

    public Texture GameWorldTexture;

    public Vector2 OffsetStart;
}
