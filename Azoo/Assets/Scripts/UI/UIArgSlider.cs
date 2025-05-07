using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIArgSlider : MonoBehaviour
{
        public string RelatedArg;
        public int MaxValue;

        public Slider slider;

        private void Awake()
        {
                slider = GetComponent<Slider>();
                slider.maxValue = MaxValue;
                slider.value = 0;
        }

        // Update is called once per frame
        void Update()
        {
                slider.value = SaveManager.Instance.Get(RelatedArg);
        }
}
