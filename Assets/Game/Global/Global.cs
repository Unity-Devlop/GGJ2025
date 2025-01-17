using System;
using System.Runtime.CompilerServices;
using Game.Flow;
using UnityEngine;
using UnityToolkit;
using Random = System.Random;

namespace Game
{
    public class Global : MonoSingleton<Global>
    {
        #region Fast Access
        
        private static TypeEventSystem _event;

        /// <summary>
        /// 事件系统
        /// </summary>
        public static TypeEventSystem Event
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (_event == null)
                {
                    _event = new TypeEventSystem();
                }

                return _event;
            }
        }

        private DataSystem _data;
        private GameConfig _config;
        private SystemLocator _systemLocator;
        private CameraSystem _cameraSystem;
        private AudioSystem _audio;
        
        public static DataSystem Data => Singleton._data;
        public static SystemLocator System => Singleton._systemLocator;
        public static GameConfig Config => Singleton._config;
        public static CameraSystem cameraSystem => Singleton._cameraSystem;
        public static AudioSystem Audio => Singleton._audio;

        [ThreadStatic] private static Random _random;

        public static Random random
        {
            get
            {
                if (_random == null)
                {
                    _random = new Random();
                }

                return _random;
            }
        }


        public static string DeviceAddress => SystemInfo.deviceUniqueIdentifier;

        #endregion


        protected override bool DontDestroyOnLoad() => true;

        protected override void OnInit()
        {
            _event = new TypeEventSystem();

            ToolkitLog.writeLog = false;
            ToolkitLog.infoAction = GameLogger.Log.Information;
            ToolkitLog.warningAction = GameLogger.Log.Warning;
            ToolkitLog.errorAction = GameLogger.Log.Error;

            
            UIRoot.Singleton.UIDatabase.Loader = new AddressablesUILoader();

            _systemLocator = new SystemLocator();

            _systemLocator.Register<CameraSystem>(GetComponentInChildren<CameraSystem>());
            _systemLocator.Register<GameConfig>(GetComponentInChildren<GameConfig>());
            _systemLocator.Register<AudioSystem>(GetComponentInChildren<AudioSystem>());
            _systemLocator.Register<SceneSystem>(GetComponentInChildren<SceneSystem>());
            _systemLocator.Register<GameFlow>(GetComponentInChildren<GameFlow>());
            _systemLocator.Register<DataSystem>(new DataSystem());
            _systemLocator.Register<ResourceSystem>(GetComponentInChildren<ResourceSystem>());
            _systemLocator.Register<LocalizationSystem>(GetComponentInChildren<LocalizationSystem>());
            _systemLocator.Register<UserTraceSystem>();


            _cameraSystem = Get<CameraSystem>();
            _audio = Get<AudioSystem>();
            _data = Get<DataSystem>();
            _config = Get<GameConfig>();
            
            Get<GameFlow>().Run();
        }

        public void Update()
        {
            foreach (var system in _systemLocator.systems)
            {
                if (system is IOnUpdate onUpdate)
                {
                    onUpdate.OnUpdate(Time.deltaTime);
                }
            }
        }


        protected override void OnDispose()
        {
            _event = null;
            _systemLocator.Dispose();
            _systemLocator = null;
        }


        #region Static Fast Access

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TSystem Get<TSystem>() where TSystem : class, ISystem
        {
            return Singleton._systemLocator.Get<TSystem>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Add<TSystem>(TSystem system) where TSystem : class, ISystem
        {
            Singleton._systemLocator.Register(system);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Remove<TSystem>() where TSystem : class, ISystem
        {
            Singleton._systemLocator.UnRegister<TSystem>();
        }

        #endregion
    }
}