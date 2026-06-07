using System.Collections;
using UnityEngine;

namespace DvMod.Paperwork
{
    public class SfxHost : MonoBehaviour
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
                DontDestroyOnLoad(go);

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

        public static IEnumerator PlayOneShotDelayed(AudioClip clip, float delaySeconds)
        {
            yield return new WaitForSeconds(delaySeconds);

            var src = Instance.GetComponent<AudioSource>();

            const float volume = 1f;
            src.PlayOneShot(clip, volume);
        }
    }
}
