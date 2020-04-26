using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu(menuName: "Image Effects/Color Adjustments/Brightness")]
public class Brightness : MonoBehaviour
{
    [Range(min: 0.5f, max: 2f)]
    public float brightness = 1f;

    private Material m_Material;

    /// Provides a shader property that is set in the inspector
    /// and a material instantiated from the shader
    public Shader shaderDerp;


    private Material material
    {
        get
        {
            if (m_Material == null)
            {
                m_Material = new Material(shader: shaderDerp);
                m_Material.hideFlags = HideFlags.HideAndDontSave;
            }

            return m_Material;
        }
    }

    private void Start()
    {
        // Disable if we don't support image effects
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        // Disable the image effect if the shader can't
        // run on the users graphics card
        if (!shaderDerp || !shaderDerp.isSupported) enabled = false;
    }


    private void OnDisable()
    {
        if (m_Material) DestroyImmediate(obj: m_Material);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        material.SetFloat(name: "_Brightness", value: brightness);
        Graphics.Blit(source: source, dest: destination, mat: material);
    }
}