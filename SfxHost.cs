using UnityEngine;

namespace DvMod.Paperwork
{
    internal class SfxHost : MonoBehaviour
    {
        private static SfxHost? _instance;
        private AudioSource? _source;

        public static SfxHost Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var go = new GameObject("DvMod_Paperwork_SfxHost");
                UnityEngine.Object.DontDestroyOnLoad(go);

                _instance = go.AddComponent<SfxHost>();
                _instance.Init();

                return _instance;
            }
        }

        private void Init()
        {
            if (_source != null)
                return;

            _source = gameObject.AddComponent<AudioSource>();
            _source.spatialBlend = 0f;
            _source.playOnAwake = false;
        }
    }
}
