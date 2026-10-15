using System;
using System.Collections.Generic;
using UnityEngine;

namespace UtilsToolbox.Utils.TimeBased.UpdateHelper
{
    public class UpdateManager : MonoBehaviour
    {
        private static readonly Dictionary<UpdateType, List<Action>> UpdateLists = 
            new Dictionary<UpdateType, List<Action>>();

        [field: SerializeField] public bool Paused { get; protected set; }
        [SerializeField] public float _timeScale = 1;
        
        public float TimeScale
        {
            get => _timeScale;
            set => _timeScale = value;
        }

#if UNITY_EDITOR
        protected virtual void OnEnable()
        {
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        protected virtual void OnDisable()
        {
            UnityEditor.EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                UpdateLists.Clear();
            }
        }
#endif

        private void FixedUpdate()
        {
            if (Paused == false)
            {
                InvokeActions(UpdateType.FixedUpdate);
            }

            InvokeActions(UpdateType.FixedUpdateUnscaled);
        }

        private void Update()
        {
            if (Paused == false)
            {
                InvokeActions(UpdateType.Update);
            }

            InvokeActions(UpdateType.UpdateUnscaled);
        }

        private void LateUpdate()
        {
            if (Paused == false)
            {
                InvokeActions(UpdateType.LateUpdate);
            }

            InvokeActions(UpdateType.LateUpdateUnscaled);
        }

        private static void InvokeActions(UpdateType updateType)
        {
            if (UpdateLists.TryGetValue(updateType, out List<Action> actions))
            {
                for (int i = 0; i < actions.Count; i++)
                {
                    actions[i]?.Invoke();
                }
            }
        }

        #region PUBLIC METHODS
        public static void AddListener(UpdateType updateType, Action updateAction)
        {
            if (updateAction == null)
            {
                return;
            }

            if (!UpdateLists.ContainsKey(updateType))
            {
                UpdateLists[updateType] = new List<Action>();
            }

            List<Action> actions = UpdateLists[updateType];
            if (!actions.Contains(updateAction))
            {
                actions.Add(updateAction);
            }
        }

        public static void RemoveListener(UpdateType updateType, Action updateAction)
        {
            if (updateAction == null)
            {
                return;
            }

            if (UpdateLists.TryGetValue(updateType, out List<Action> actions))
            {
                actions.Remove(updateAction);
            }
        }
        #endregion
    }
}