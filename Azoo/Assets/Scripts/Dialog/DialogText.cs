using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DialoSentence
{
        public string Name;
        [TextArea(3, 10)]
        public string Content;
}

[System.Serializable]
public struct DialogChoice
{
        public string Name;
        public DialogText NextDialog;
}

public enum DialogConditionType
{
        None,
        Equal,
        NotEqual,
        Greater,
        Less,
        EqualOrGreater,
        EqualOrLess,
}

[System.Serializable]
public struct DialogCondition
{
        public string ArgName;
        public DialogConditionType ConditionType;
}

public enum DialogEffectType
{
        None,
        Set,
        Add,
        Subtract,
}
[System.Serializable]
public struct DialogEffect
{
        public string ArgName;
        public int value;
        public DialogEffectType EffectType;
}


[CreateAssetMenu(fileName = "DialogText", menuName = "DialogText")]
public class DialogText : ScriptableObject
{
        public DialogCondition[] Conditions;
        public DialogEffect[] Effects;
        public List<DialoSentence> Sentences;
        public List<DialogChoice> Choices;

        public bool CanShow()
        {
                foreach (var condition in Conditions)
                {
                        int value = SaveManager.Instance.Get(condition.ArgName);
                        switch (condition.ConditionType)
                        {
                                case DialogConditionType.Equal:
                                        if (value != 0) return false;
                                        break;
                                case DialogConditionType.NotEqual:
                                        if (value == 0) return false;
                                        break;
                                case DialogConditionType.Greater:
                                        if (value <= 0) return false;
                                        break;
                                case DialogConditionType.Less:
                                        if (value >= 0) return false;
                                        break;
                                case DialogConditionType.EqualOrGreater:
                                        if (value < 0) return false;
                                        break;
                                case DialogConditionType.EqualOrLess:
                                        if (value > 0) return false;
                                        break;
                        }
                }
                return true;
        }

        public void ApplyEffects()
        {
                foreach (var effect in Effects)
                {
                        int value = SaveManager.Instance.Get(effect.ArgName);
                        switch (effect.EffectType)
                        {
                                case DialogEffectType.Set:
                                        SaveManager.Instance.Set(effect.ArgName, effect.value);
                                        break;
                                case DialogEffectType.Add:
                                        SaveManager.Instance.Set(effect.ArgName, value + effect.value);
                                        break;
                                case DialogEffectType.Subtract:
                                        SaveManager.Instance.Set(effect.ArgName, value - effect.value);
                                        break;
                                default:
                                        break;
                        }
                }
        }
}