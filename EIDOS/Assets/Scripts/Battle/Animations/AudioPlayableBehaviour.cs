using UnityEngine;
using UnityEngine.Playables;

namespace EIDOS.Battle.Animations
{
    public class AudioPlayableBehaviour : PlayableBehaviour
    {
        public AudioClip clip;
        public ExposedReference<AudioSource> audioSource;

        private AudioSource resolvedAudioSource;

        public override void OnGraphStart(Playable playable)
        {
            IExposedPropertyTable resolver = playable.GetGraph().GetResolver();
            resolvedAudioSource = audioSource.Resolve(resolver);
        }

        public void PlayAudioClip()
        {
            if (clip == null || clip.length == 0 || resolvedAudioSource == null) return;
            
            resolvedAudioSource.PlayOneShot(clip);
        }
    }
}