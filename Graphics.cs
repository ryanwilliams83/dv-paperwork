using System;
using UnityEngine;

namespace DvMod.Paperwork
{
    public static class Graphics
    {
        public static Texture2D Composite(Texture2D a, Texture2D b, float opacity)
        {
            Paperwork.LogTrace($"{nameof(Graphics)}.{nameof(Composite)}()");

            if (a == null)
                throw new ArgumentNullException(nameof(a));

            if (b == null)
                throw new ArgumentNullException(nameof(b));

            var w = a.width;
            var h = a.height;

            var blendMaterial = new Material(Shader.Find(@"Sprites/Default"));
            blendMaterial.color = new Color(1f, 1f, 1f, opacity);

            RenderTexture rt = RenderTexture.GetTemporary(w, h, 0);
            UnityEngine.Graphics.Blit(a, rt);
            UnityEngine.Graphics.Blit(b, rt, blendMaterial);

            RenderTexture.active = rt;
            Texture2D result = new Texture2D(w, h, TextureFormat.RGBA32, false);
            result.ReadPixels(new Rect(0, 0, w, h), 0, 0);
            result.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            return result;
        }
    }
}
