using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.UI;

namespace Incode.Prototype
{
    public class HeroUI : MonoBehaviour
    {
        [SerializeField] private RectTransform healthbarRectTransform = null;
        [SerializeField] private Image healthbarFill = null;

        private int lastHealthValue = -1;

        private UnitEntity unitEntity = null;

        private Camera mainCamera = null;


        void Awake()
        {
            unitEntity = this.GetComponent<UnitEntity>();

            mainCamera = Camera.main;
        }

        void Update()
        {
            healthbarRectTransform.rotation = Quaternion.LookRotation(-mainCamera.transform.forward);

            if (lastHealthValue != unitEntity.UnitStatus.Health)
            {
                healthbarFill.fillAmount = (float)unitEntity.UnitStatus.Health / unitEntity.UnitStatus.MaxHealth;
                lastHealthValue = unitEntity.UnitStatus.Health;
            }
        }
    }
}