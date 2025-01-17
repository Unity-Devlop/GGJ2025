using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityToolkit;
using Debug = UnityEngine.Debug;

namespace Game
{
    public readonly struct BGMCallBackInfo
    {
        public readonly string bgmA;
        public readonly string bgmB;
        public readonly Func<EventInstance, bool> condition;
        public readonly Action action;

        public BGMCallBackInfo(string bgmA, string bgmB, Func<EventInstance, bool> condition, Action action)
        {
            this.bgmA = bgmA;
            this.bgmB = bgmB;
            this.condition = condition;
            this.action = action;
        }
    }

    public class AudioSystem : MonoBehaviour, ISystem, IOnUpdate, IOnInit
    {
        [SerializeField] private AssetReference bank;
        [SerializeField] private AssetReference bankStrings;
        private List<BGMCallBackInfo> _callBackInfos;

        private Dictionary<string, EventInstance> _bgmInstances;

        public void OnInit()
        {
            TextAsset bankText = Addressables.LoadAssetAsync<TextAsset>(bank).WaitForCompletion();
            TextAsset bankStringsText = Addressables.LoadAssetAsync<TextAsset>(bankStrings).WaitForCompletion();

            RuntimeManager.LoadBank(bankText, false);
            RuntimeManager.LoadBank(bankStringsText, false);

            _bgmInstances = new Dictionary<string, EventInstance>(8);
            _callBackInfos = new List<BGMCallBackInfo>(8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IsBGMPlaying(string path)
        {
            if (!_bgmInstances.TryGetValue(path, out var instance)) return false;
            var status = instance.getPaused(out var paused);
            Debug.Assert(status == RESULT.OK);
            return !paused;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetBGM(string path, out EventInstance instance)
        {
            return _bgmInstances.TryGetValue(path, out instance);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PlayBGM(string path, out EventInstance instance)
        {
            if (_bgmInstances.TryGetValue(path, out instance))
            {
                Debug.Assert(instance.isValid());
                instance.start();
                return true;
            }

            instance = RuntimeManager.CreateInstance(path);
            Debug.Assert(instance.isValid());
            instance.start();
            _bgmInstances.Add(path, instance);
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool ResumeBGM(string path)
        {
            if (!_bgmInstances.TryGetValue(path, out var eventInstance)) return false;
            Debug.Assert(eventInstance.isValid());
            return eventInstance.setPaused(false) == RESULT.OK;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PauseBGM(string path)
        {
            if (!_bgmInstances.TryGetValue(path, out var eventInstance)) return false;
            Debug.Assert(eventInstance.isValid());
            return eventInstance.setPaused(true) == RESULT.OK;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseAllBGMExpect(string path)
        {
            foreach (var (key, eventInstance) in _bgmInstances)
            {
                if (key == path) continue;
                Debug.Assert(eventInstance.isValid());
                eventInstance.setPaused(true);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PauseAllBGM()
        {
            foreach (var (_, eventInstance) in _bgmInstances)
            {
                Debug.Assert(eventInstance.isValid());
                eventInstance.setPaused(true);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisposeAllBGMExpect(string path)
        {
            EventInstance keep = default;
            bool find = false;
            foreach (var (key, eventInstance) in _bgmInstances)
            {
                if (key == path)
                {
                    find = true;
                    keep = eventInstance;
                    continue;
                }

                Debug.Assert(eventInstance.isValid());
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
                eventInstance.clearHandle();
            }

            _bgmInstances.Clear();
            if (find)
            {
                _bgmInstances.Add(path, keep);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisposeAllBGM()
        {
            foreach (var (key, eventInstance) in _bgmInstances)
            {
                Debug.Assert(eventInstance.isValid());
                eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                eventInstance.release();
                eventInstance.clearHandle();
            }

            _bgmInstances.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DisposeBGM(string path)
        {
            if (!_bgmInstances.TryGetValue(path, out var eventInstance)) return;
            Debug.Assert(eventInstance.isValid());
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
            eventInstance.clearHandle();
            _bgmInstances.Remove(path);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool PlayOneShot(in string path, in Vector3 position, out EventInstance instance)
        {
            instance = RuntimeManager.CreateInstance(path);
            Debug.Assert(instance.isValid());
            instance.set3DAttributes(position.To3DAttributes());
            instance.start();
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlayOneShot(string path, in Vector3 position)
        {
            RuntimeManager.PlayOneShot(path, position);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PlayOneShot(string path)
        {
            RuntimeManager.PlayOneShot(path);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddCallBack(ref BGMCallBackInfo info)
        {
            GameAssert.EditorAssert(RuntimeManager.GetEventDescription(info.bgmA).isValid());
            GameAssert.EditorAssert(RuntimeManager.GetEventDescription(info.bgmB).isValid());
            _callBackInfos.Add(info);
        }

        public void Dispose()
        {
            DisposeAllBGM();
        }


        public void OnUpdate(float deltaTime)
        {
            for (int i = _callBackInfos.Count - 1; i >= 0; i--)
            {
                var info = _callBackInfos[i];
                if (GetBGM(info.bgmA, out var instance) && info.condition(instance))
                {
                    info.action();
                    _callBackInfos.RemoveAt(i);
                }
            }
        }
    }
}