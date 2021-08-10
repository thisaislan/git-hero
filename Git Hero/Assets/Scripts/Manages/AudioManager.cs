using Githero.Bases;
using System;
using System.Collections;
using UnityEngine;

namespace Githero.Managers
{

    public sealed class AudioManager : ManagerBase<AudioManager>
    {
        private const float InitialVolumeBackgroundAudioSource = 0.5f;
        private const float MaxVolumeBackgroundAudioSource = 0.8f;
        private const float MinVolumeBackgroundAudioSource = 0f;

        private const float SoundDelayBackgroundAudioSource = 0.7f;
        private const float DeltaVolumeBackgroundAudioSource = 0.1f;

        AudioSource backgroudAudioSource;

        public void PlayBackgroundAudioSource(
            AudioSource backgroudAudioSource,
            MonoBehaviour monoBehaviour)
        {
            this.backgroudAudioSource = backgroudAudioSource;

            this.backgroudAudioSource.loop = true;
            this.backgroudAudioSource.volume = InitialVolumeBackgroundAudioSource;

            PlayAudioSource(this.backgroudAudioSource);

            monoBehaviour.StartCoroutine(ProgresBackgroundAudioSourceVolume(
                    MaxVolumeBackgroundAudioSource,
                    monoBehaviour)
                );
        }

        public void StopBackgroundAudioSource(MonoBehaviour monoBehaviour)
        {
            monoBehaviour.StartCoroutine(ProgresBackgroundAudioSourceVolume(
                    MinVolumeBackgroundAudioSource,
                    monoBehaviour)
                );
        }

        public void PlayAudioSource(AudioSource source) =>
            source.Play();

        private IEnumerator ProgresBackgroundAudioSourceVolume(
            float volume,
            MonoBehaviour monoBehaviour)
        {
            yield return new WaitForSeconds(SoundDelayBackgroundAudioSource);

            float currentBackgroudAudioSourceVolume = backgroudAudioSource.volume;

            if (currentBackgroudAudioSourceVolume < volume)
            {
                AddPartialBackgroundVolume(
                        volume,
                        DeltaVolumeBackgroundAudioSource,
                        monoBehaviour
                    );
            }
            else if (currentBackgroudAudioSourceVolume > volume)
            {
                AddPartialBackgroundVolume(
                        volume,
                        -DeltaVolumeBackgroundAudioSource,
                        monoBehaviour
                    );
            }
        }

        private void AddPartialBackgroundVolume(
            float volume,
            float partialBackgroundVolume,
            MonoBehaviour monoBehaviour)
        {
            this.backgroudAudioSource.volume =
                    (float)Math.Round(backgroudAudioSource.volume + partialBackgroundVolume, 1);

            monoBehaviour.StartCoroutine(
                    ProgresBackgroundAudioSourceVolume(volume, monoBehaviour)
                );
        }

    }

}