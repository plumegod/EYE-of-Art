using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace aPlume.Input
{
    public class SceneInputManager : MonoBehaviour
    {
        [Header("主要操控模式")][SerializeField] private InputReader inputReader;
        public InputReader InputReader => inputReader;
        public static SceneInputManager Instance { get; private set; }
        
        private void Awake()
        {
            Instance = this;
        }

        // 寻找按键
        public InputAction FindAction(string actionName)
        {
            return Instance.inputReader.GameInput.FindAction(actionName);
        }
    }
}

