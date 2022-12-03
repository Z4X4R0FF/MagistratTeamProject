using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.InputSystem
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager instance;

        private Input[] inputs;
        [SerializeField] private float Sensivity;

        private readonly Dictionary<int, float> lastValues = new Dictionary<int, float>();
        private readonly InputType[] allInputsTypes = Enum.GetValues(typeof(InputType)).Cast<InputType>().ToArray();
        private readonly Dictionary<int, string> inputNames = new Dictionary<int, string>();

        //private static readonly List<int> AxisInputs = new List<int>
        //{
        //    (int) InputType.Horizontal,
        //    (int) InputType.Vertical
        //};

        public Action<float> SubscribeToInputEvent(InputType type, Action<float> action, bool callWithLastInput = false)
        {
            if (inputs == null || inputs.Length == 0)
                FillInputsArray();

            inputs[(int)type].Subscribe(action, callWithLastInput);
            return action;
        }

        public void UnsubscribeFromInputEvent(InputType type, Action<float> action)
        {
            inputs[(int)type].Unsubscribe(action);
        }

        public void RaiseInputEvent(InputType type, float value)
        {
            float result = Mathf.Clamp(value, -1, 1);
            Input input = inputs[(int)type];
            if (result != input.GetLastValue())
            {
                input.Raise(result);
            }
        }

        public void RaiseLogicalInputEvent(InputType type, bool value)
        {
            float result = value ? 1f : 0f;
            Input input = inputs[(int)type];
            if (result != input.GetLastValue())
            {
                input.Raise(result);
            }
        }

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            FillDictionary();
        }

        private void Update()
        {
            foreach (InputType inputType in allInputsTypes)
            {
                if (inputType == InputType.None)
                {
                    continue;
                }

                float inputValue = UnityEngine.Input.GetAxis(inputNames[(int)inputType]);
                inputValue = Mathf.Clamp(inputValue, -1, 1);

                //if (AxisInputs.Contains((int)inputType))
                //{
                //    inputValue = Mathf.Lerp(lastValues[(int)inputType], inputValue, 0.1f);
                //    RaiseInputEvent(inputType, inputValue * Sensivity);
                //}
                //else
                //{
                    RaiseInputEvent(inputType, inputValue);
                //}

                lastValues[(int)inputType] = inputValue;
            }
        }

        private void FillInputsArray()
        {
            Array values = Enum.GetValues(typeof(InputType));

            InputType lastType = InputType.None;

            for (int i = 0; i < values.Length; i++)
            {
                InputType type = (InputType)values.GetValue(i);
                if (type > lastType)
                {
                    lastType = type;
                }
            }

            inputs = new Input[(int)lastType + 1];

            for (int i = 0; i < values.Length; i++)
            {
                InputType type = (InputType)values.GetValue(i);

                if (type == InputType.None)
                {
                    continue;
                }

                inputs[(int)type] = new Input(type);
            }
        }

        private void FillDictionary()
        {
            foreach (InputType value in allInputsTypes)
            {
                lastValues.Add((int)value, 0);
                inputNames.Add((int)value, value.ToString());
            }
        }
    }

    public class Input
    {
        public InputType inputType;
        private Action<float> actions;
        private float lastValue;

        public Input(InputType type)
        {
            inputType = type;
        }

        public void Raise(float value)
        {
            lastValue = value;

            if (actions != null)
            {
                actions(value);
            }
        }

        public void Subscribe(Action<float> action, bool callWithLastInput)
        {
            actions += action;

            if (callWithLastInput)
            {
                action(lastValue);
            }
        }

        public void Unsubscribe(Action<float> action)
        {
            if (action != null && actions != null)
            {
                actions -= action;
            }
        }

        public float GetLastValue()
        {
            return lastValue;
        }
    }

    public enum InputType
    {
        None = -1,
        Horizontal = 0,
        Vertical = 1,
        Jump = 2,
        MouseHorizontal = 3,
        MouseVertical = 4,
    }
}