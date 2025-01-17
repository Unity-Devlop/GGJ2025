using System;
using System.Collections.Generic;
using System.IO;
using LiteDB;
using Newtonsoft.Json;
using UnityEngine;
using UnityToolkit;

namespace Game
{
    public class DataSystem : ISystem, IOnInit
    {
        public LiteDatabase database { get; private set; }

        private static string prefixPath
        {
            get
            {
#if UNITY_EDITOR
                return Application.dataPath + "/Editor Default Resources/";
#else
                return Application.persistentDataPath + "/";
#endif
            }
        }

        internal static string dbPath
        {
            get
            {
#if UNITY_EDITOR
                return prefixPath + "data.db";
#else
                return prefixPath + "data.db";
#endif
            }
        }

        private bool _initialized;

        public void OnInit()
        {
            if (_initialized) return;
            database = new LiteDatabase(dbPath);
            _initialized = true;
        }


        public void Dispose()
        {
            // TODO 保存数据
            if (database != null)
            {
                database.Dispose();
                database = null;
            }

            _initialized = false;
        }


        public T GetOrDefault<T>(int id) where T : new()
        {
            if (typeof(IJsonData).IsAssignableFrom(typeof(T)))
            {
                return GetOrDefaultJson<T>(id);
            }

            return GetOrDefaultLiteDB<T>(id);
        }

        private T GetOrDefaultJson<T>(int id) where T : new()
        {
            string path = prefixPath + typeof(T).Name + "-" + id + ".json";
            if (!File.Exists(path))
            {
                GameLogger.Log.Information("{data-system},GetOrDefault<Json> {path} {null}", nameof(DataSystem), path,
                    null);
                return new T();
            }

            var str = File.ReadAllText(path);
            GameLogger.Log.Information("{data-system},GetOrDefault<Json> {path} {str}", nameof(DataSystem), path, str);
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(str);
                // var str2 = JsonConvert.SerializeObject(obj);
                // GameLogger.Log($"{str2}");
                return obj;
            }
            catch (JsonSerializationException e)
            {
                GameLogger.Log.Warning("{data-system},GetOrDefault<Json> {path} {e}", nameof(DataSystem), path, e);
                File.Delete(path);
                return new T();
            }
        }

        private T GetOrDefaultLiteDB<T>(int id) where T : new()
        {
            throw new NotImplementedException();
            // var collection = database.GetCollection<T>();
            // var result = collection.FindById(id);
            // GameLogger.Log($"[{nameof(DataSystem)}]:GetOrDefault<LiteDB> {id} {result}");
            // if (result == null)
            // {
            //     result = new T();
            //     GameLogger.Log($"[{nameof(DataSystem)}]:Create {id} {result}");
            //     collection.Insert(id, result);
            // }
            // else
            // {
            //     GameLogger.Log($"[{nameof(DataSystem)}]:Get {id} {result}");
            // }
            //
            // return result;
        }

        public void Save<T>(int id, T data)
        {
            if (typeof(IJsonData).IsAssignableFrom(typeof(T)))
            {
                SaveJson(id, data);
            }
            else
            {
                SaveLiteDB(id, data);
            }
        }

        private void SaveLiteDB<T>(int id, T data)
        {
            // GameLogger.Log($"[{nameof(DataSystem)}]:Save {id} {data}");
            // var collection = database.GetCollection<T>();
            // collection.Upsert(id, data);
        }

        private void SaveJson<T>(int id, T data)
        {
            string path = prefixPath + typeof(T).Name + "-" + id + ".json";
            var str = JsonConvert.SerializeObject(data, Formatting.Indented);
// #if UNITY_EDITOR
            GameLogger.Log.Information("{data-system},Save<Json> {path} {str}", nameof(DataSystem), path, str);
// #endif
            File.WriteAllText(path, str);
        }

        #region FastAccess

        internal static DataSystem Shared
        {
            get
            {
                if (Application.isPlaying)
                {
                    GameLogger.Log.Error(new System.Exception("请勿在运行时访问DataSystem.Shared 仅仅在编辑器中使用"),
                        "{data-system},Shared", null);
                }

                if (_shared != null) return _shared;
                _shared = new DataSystem();
                _shared.OnInit();
                return _shared;
            }
        }

        internal static DataSystem _shared;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void ResetStatic()
        {
            if (_shared != null)
            {
                _shared.Dispose();
            }

            _shared = null;
        }

        #endregion
    }
}