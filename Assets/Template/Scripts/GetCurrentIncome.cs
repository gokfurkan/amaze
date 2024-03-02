﻿using Template.Scripts.Scriptables;
using TMPro;
using UnityEngine;

namespace Template.Scripts
{
    public class GetCurrentIncome : MonoBehaviour
    {
        [SerializeField] private IncomeTextType incomeTextType;

        private void Start()
        {
            InitIncomeTextType();
        }

        private void InitIncomeTextType()
        {
            TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
            EconomyOptions economyOptions = InfrastructureManager.instance.gameSettings.economyOptions;

            switch (incomeTextType)
            {
                case IncomeTextType.Win:
                    text.text = economyOptions.winIncome.ToString();
                    break;
            }
        }
    }
}