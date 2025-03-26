using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

        public GameObject InteractIndicator;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
                if (InteractIndicator)
                {
                        InteractIndicator.SetActive(PlayerController.Instance.viewhandler.target.Count > 0);
                }
        }
}
