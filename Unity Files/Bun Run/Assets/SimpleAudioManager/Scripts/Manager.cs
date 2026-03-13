using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UIElements;

namespace SimpleAudioManager
{
    public class Manager : MonoBehaviour
    {
        #region PROPERTIES

        /// <summary>
        /// Singleton
        /// </summary>
        public static Manager instance => _instance;
        private static Manager _instance = null;

        /// <summary>
        /// The attached audio source
        /// </summary>
        [Header("CONFIGURATIONS")]
        [Tooltip("The audio source prefab which will be used in the audio source pool.")] public GameObject audioSourcePrefab = null;

        [Tooltip("Should the current song loop?")] public bool loopCurrentSong = true;
        
        /// <summary>
        /// The time before either a non-looping clip ends or the next loop of a looping clip begins
        /// </summary>
        public float clipTimeRemaining 
        {
            get
            {
                if (_activeLoopInstances.Count == 0) return 0;
                return (loopCurrentSong ? _activeLoopInstances[_activeLoopInstances.Count - 1].end : _activeLoopInstances[_activeLoopInstances.Count - 1].tail) - Time.time;
            }
        }
        [Tooltip("Should the manager play the first song on awake?")] public bool playOnAwake = true;
        [Tooltip("The maximum volume for the audio clips.")][Range(0f, 1f)] public float maxVolume = 1f;
        [Tooltip("The amount of time it will take for different songs to blend between one-another.")] public float defaultSongBlendDuration = 1f;
        [Tooltip("The amount of time it will take for different intensities of the same song to blend between one-another.")] public float defaultIntensityBlendDuration = 1f;

        [Space(8f)]
        /// <summary>
        /// The available songs for the manager
        /// </summary>
        [Tooltip(
            "The list of available songs for the manager to play.\n" +
            "-To create a new song:\n" +
            "  -Right Click and select:\n" +
            "    Create->SimpleAudioManager->Song\n" +
            "  -Add the intensity clips or any other desired clips\n" +
            "  -Set the reverb tail time\n" +
            "    (Seconds before the end of a clip to loop it)\n" +
            "    (Shown in parentheses on Ovani Folders)\n" +
            "  -Drag & Drop your songs onto this list")] [SerializeField] private List<Song> _songs = new List<Song>();

        #endregion

        #region PUBLIC METHODS

        /// <summary>
        /// Direct Accessor for Intensity - uses no fading, cancels current intensity transitions.
        /// Range: 0f -> 1.0f
        /// </summary>
        public float Intensity 
        {
            get => _defaultIntensity;
            set
            {
                if (IntensityFade != null)
                {
                    StopCoroutine(IntensityFade);
                    IntensityFade = null;
                }
                _defaultIntensity = Mathf.Clamp01(value);
            }
        }
        [Tooltip("Current/Default Intensity for the Manager")][SerializeField][Range(0,1)]private float _defaultIntensity;

        /// <summary>
        /// Legacy Shorthand Crossfade Intensity for <see cref="Manager.defaultIntensityBlendDuration"/> seconds
        /// Range: 0 -> 2 Inclusive
        /// </summary>
        public void SetIntensity(int pIntensity) => SetIntensity((float)(pIntensity / 2f));

        /// <summary>
        /// Shorthand Crossfades Intensity for <see cref="Manager.defaultIntensityBlendDuration"/> seconds
        /// Range: 0f -> 1.0f
        /// </summary>
        public void SetIntensity(float pIntensity) => SetIntensity(pIntensity, defaultIntensityBlendDuration);

        /// <summary>
        /// Legacy Crossfades Intensity for the current song
        /// Range: 0 -> 2 Inclusive
        /// </summary>
        public void SetIntensity(int pIntensity, float pBlendOutDuration, float pBlendInDuration)
            => SetIntensity((float)(pIntensity / 2f), pBlendOutDuration);

        /// <summary>
        /// Crossfades Intensity for the current song
        /// Range: 0f -> 1.0f
        /// </summary>
        public void SetIntensity(float pIntensity, float pBlendDuration)
        {
            pIntensity = Mathf.Clamp01(pIntensity);
            if (IntensityFade != null)
            {
                StopCoroutine(IntensityFade);
                IntensityFade = null;
            }
            if (pBlendDuration == 0)
            {
                Intensity = pIntensity;
                return;
            }
            IEnumerator fader(float start, float end)
            {
                float wait = pBlendDuration;
                float remainingWait = pBlendDuration;
                while (remainingWait > 0)
                {
                    remainingWait -= Time.deltaTime;

                    _defaultIntensity = Mathf.Lerp(start, end, (wait - remainingWait) / wait);
                    yield return null;
                }
                _defaultIntensity = end;
            }
            IntensityFade = StartCoroutine(fader(this.Intensity, pIntensity));
        }
        Coroutine IntensityFade;

        /// <summary>
        /// Plays the specified song and attempts to match the current intensity
        /// </summary>
        public void PlaySong(int pSong) {
            PlaySong(new PlaySongOptions()
            {
                song = pSong,
                intensity = -1,
                floatIntensity = this.Intensity,
                blendInTime = defaultSongBlendDuration,
                blendOutTime = defaultSongBlendDuration,
            });
        }

        /// <summary>
        /// Plays the specified song
        /// </summary>
        public void PlaySong(PlaySongOptions pOptions)
        {
            if (_activeLoopInstances.Count > 0)
            {
                var mostRecentLooper = _activeLoopInstances[_activeLoopInstances.Count - 1];
                mostRecentLooper.SetFadeOut(pOptions.blendOutTime);
            }
            var looper = new LoopInstance(_songs[pOptions.song], pOptions.startTime);
            looper.SetFadeIn(pOptions.blendInTime);
        }

        /// <summary>
        /// Play song options
        /// </summary>
        public struct PlaySongOptions
        {
            public int song;
            /// <summary>
            /// Legacy Intensity Specifier
            /// Set to -1 to use <see cref="floatIntensity"> instead.
            /// </summary>
            public int intensity;
            /// <summary>
            /// Dynamic intensity Specifier
            /// Range: 0f -> 1.0f
            /// Disabled by default, enable by setting <see cref="intensity"/> to -1.
            /// </summary>
            public float floatIntensity;
            public float startTime;
            public float blendOutTime;
            public float blendInTime;
        }

        /// <summary>
        /// Stops the current song playing
        /// </summary>
        public void StopSong(float pFadeOutDuration)
        {
            if (_activeLoopInstances.Count > 0)
            {
                var mostRecentLooper = _activeLoopInstances[_activeLoopInstances.Count - 1];
                mostRecentLooper.SetFadeOut(pFadeOutDuration);
            }
        }

        #endregion

        #region PRIVATE METHODS

        private List<LoopInstance> _activeLoopInstances = new();
        private class LoopInstance
        {
            public LoopInstance(Song song, float startTime)
            {
                if (song.intensityClips.Count == 0) throw new InvalidOperationException("[SimpleAudioManager] Error: Attempted to play a song with Zero clips! song: " + song.name);

                InstanceRoot = new GameObject("LoopInstance" + UnityEngine.Random.Range(0, 999) + ":" + song.name);
                InstanceRoot.transform.SetParent(Manager.instance.gameObject.transform);
                sources = new AudioSource[song.intensityClips.Count];
                for (int i = 0; i < song.intensityClips.Count; i++)
                {
                    AudioClip clip = song.intensityClips[i];
                    var newPlr = Instantiate(Manager.instance.audioSourcePrefab);
                    newPlr.transform.parent = InstanceRoot.transform;
                    var newPlrSrc = newPlr.GetComponent<AudioSource>();
                    newPlrSrc.clip = clip;
                    sources[i] = newPlrSrc;
                    newPlrSrc.volume = 0;
                    newPlrSrc.Play();
                    newPlrSrc.time = startTime;
                }
                fadeInStart = -1; fadeInEnd = -1;
                fadeOutStart = -1; fadeOutEnd = -1;
                end = Time.time + (song.intensityClips[0].length - startTime);
                tail = end - (song.reverbTail <= 0 ? .25f : song.reverbTail);

                mySong = song;
                Manager.instance._activeLoopInstances.Add(this);
            }

            GameObject InstanceRoot;
            AudioSource[] sources;
            float fadeInStart;
            float fadeInEnd;
            float fadeOutStart;
            float fadeOutEnd;
            public float tail;
            public float end;
            Song mySong;

            public void SetFadeIn(float fadeIn)
            {
                if (fadeIn <= 0) return;
                fadeInStart = Time.time;
                fadeInEnd = Time.time + fadeIn;
            }
            public void SetFadeOut(float fadeOut)
            {
                if (fadeOut <= 0)
                {
                    end = Time.time;
                    return;
                }
                fadeOutStart = Time.time;
                fadeOutEnd = Time.time + fadeOut;
                end = Time.time + fadeOut;
                tail = -1;
            }
            public bool LoopUpdate()
            {
                if (Time.time > end)
                {
                    Destroy(InstanceRoot);
                    return true;
                }
                float primaryVolume = Manager.instance.maxVolume;
                if (fadeInStart != -1 && fadeInEnd != -1)
                {
                    if (Time.time > fadeInEnd)
                    {
                        fadeInStart = -1;
                        fadeInEnd = -1;
                    } else
                        primaryVolume = Mathf.Lerp(0, primaryVolume, (Time.time - fadeInStart) / (fadeInEnd - fadeInStart));
                }
                if (fadeOutStart != -1 && fadeOutEnd != -1)
                {
                    if (Time.time > fadeOutEnd)
                    {
                        fadeOutStart = -1;
                        fadeOutEnd = -1;
                    } else
                        primaryVolume = Mathf.Lerp(primaryVolume, 0, (Time.time - fadeOutStart) / (fadeOutEnd - fadeOutStart));
                }

                if (tail > 0 && Time.time > tail)
                {
                    tail = -1;
                    if (Manager.instance.loopCurrentSong)
                        newLoops.Add(mySong);
                }

                foreach (var src in sources)
                    src.volume = 0;

                if (Manager.instance.Intensity == 0)
                    sources[0].volume = primaryVolume;
                else if (Manager.instance.Intensity == 1)
                    sources[sources.Length - 1].volume = primaryVolume;
                else
                {
                    float longInt = Manager.instance.Intensity * (sources.Length - 1);
                    int srcA = Mathf.FloorToInt(longInt);
                    int srcB = srcA + 1;
                    sources[srcB].volume = (longInt - srcA) * primaryVolume;
                    sources[srcA].volume = (1 - (longInt - srcA)) * primaryVolume;
                }

                return false;
            }
        }

        /// <summary>
        /// Config
        /// </summary>
        private void Awake()
        {
            _instance = _instance ?? this;
            if (_instance != this)
            {
                DestroyImmediate(gameObject);
                return;
            }
            if (playOnAwake) StartCoroutine(_delay());
            IEnumerator _delay()
            {
                yield return new WaitForSecondsRealtime(0.25f);
                PlaySong(0);
            }
        }

        private static List<LoopInstance> deadLoops = new();
        private static List<Song> newLoops = new();
        private void Update()
        {
            deadLoops.Clear(); newLoops.Clear();
            foreach (var looper in _activeLoopInstances)
                if (looper.LoopUpdate())
                    deadLoops.Add(looper);
            foreach (var deadLoop in deadLoops)
                _activeLoopInstances.Remove(deadLoop);
            if (newLoops.Count > 0)
            foreach (var sng in newLoops)
                new LoopInstance(sng, 0);
        }

        /// <summary>
        /// Clear out the pseudo-singleton
        /// </summary>
        private void OnDestroy() => _instance = _instance == this ? null : _instance;

        #endregion
    }
}

/*
 * 
 * Written by Ovani Sound & Brutiful Games
 * No credit required.
 * Revision: 01/27/2026
 * 
 */