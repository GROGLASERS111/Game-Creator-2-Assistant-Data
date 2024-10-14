using System;
using UnityEngine;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.Variables;

namespace GameCreator.Runtime.VisualScripting
{
    [Title("Calculate RPG Damage Formula")]
    [Description("Performs a custom calculation based on attack, defence, resistances, and a damage multiplier.")]
    [Category("Math/RPG Formula")]

    [Keywords("RPG", "Calculation", "Formula")]
    [Image(typeof(IconDivideCircle), ColorTheme.Type.Blue)]

    [Serializable]
    public class InstructionRPGDamage : Instruction
    {
        [SerializeField]
        private PropertyGetDecimal m_Attack = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_FireDamage = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_IceDamage = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_LightningDamage = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_Defence = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_FireResistance = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_IceResistance = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_LightningResistance = new PropertyGetDecimal();
        [SerializeField]
        private PropertyGetDecimal m_Multiplier = new PropertyGetDecimal(); // Added Multiplier field
        [SerializeField]
        private PropertySetNumber m_Result = new PropertySetNumber();

        // PROPERTIES: ----------------------------------------------------------------------------

        public override string Title => $"RPG formula to calculate the total damage";

        // RUN METHOD: ----------------------------------------------------------------------------

        protected override Task Run(Args args)
        {
            double attackValue = m_Attack.Get(args);
            double fireDamageValue = m_FireDamage.Get(args);
            double iceDamageValue = m_IceDamage.Get(args);
            double lightningDamageValue = m_LightningDamage.Get(args);
            double defenceValue = m_Defence.Get(args);
            double fireResistanceValue = m_FireResistance.Get(args);
            double iceResistanceValue = m_IceResistance.Get(args);
            double lightningResistanceValue = m_LightningResistance.Get(args);
            double multiplierValue = m_Multiplier.Get(args); // Added line to get multiplier value

            double fireResistanceEffect = Math.Min(fireDamageValue, fireResistanceValue);
            double iceResistanceEffect = Math.Min(iceDamageValue, iceResistanceValue);
            double lightningResistanceEffect = Math.Min(lightningDamageValue, lightningResistanceValue);

            double totalDamage = attackValue + fireDamageValue - fireResistanceEffect + iceDamageValue - iceResistanceEffect + lightningDamageValue - lightningResistanceEffect;

            double resultValue = (totalDamage * (100.0 / (100.0 + defenceValue))) * multiplierValue; // Modified formula to include multiplier

            this.m_Result.Set(resultValue, args);

            return DefaultResult;
        }
    }
}
