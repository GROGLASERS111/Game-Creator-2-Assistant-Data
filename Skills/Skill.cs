using System.Reflection;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using GameCreator.Runtime.Characters;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ETK
{
    [System.Serializable]
    [AddComponentMenu("")]
    public class Skill
    {
        public string Title;
        public Sprite IconImage;
        public SkillType Type;
        public string Subtitle;
        public string DescriptionText;
        public Sprite DescriptionImage;
        [SerializeField] private PropertyGetGameObject target = GetGameObjectPlayer.Create();
        public float cooldown;
        [SerializeField] private GameCreator.Runtime.Stats.Attribute manaAttribute;
        public float manaCost;
        [SerializeField] private PropertySetNumber availablePoints = new PropertySetNumber();
        public int pointCost;
        public bool IsUnlocked;
        public string RequiredSkillTitle;
        public Skill RequiredSkill;
        [Space]
        [SerializeField] public RunConditionsList onCheck = new RunConditionsList();
        [Space]
        [SerializeField] public InstructionList onRun = new InstructionList();

        public enum SkillType { Active, Passive }
        private float cooldownDuration;
        private float lastUsedTime = -Mathf.Infinity;
        public float LastUsedTime { get; private set; }
        public float CooldownRemaining => Mathf.Max(0, (lastUsedTime + cooldownDuration) - Time.time);
        private bool hasBeenExecuted = false;

        public float CooldownDuration
        {
            get
            {
                return this.cooldown;
            }
        }

        public void ResetExecution()
        {
            hasBeenExecuted = false;
        }

        public void UpdateLastUsedTime(float lastUsedTime)
        {
            this.LastUsedTime = lastUsedTime;
        }


        public Skill DeepCopy()
        {
            Skill copy = new Skill();
            copy.Title = this.Title;
            copy.IconImage = this.IconImage;
            copy.Type = this.Type;
            copy.Subtitle = this.Subtitle;
            copy.DescriptionText = this.DescriptionText;
            copy.DescriptionImage = this.DescriptionImage;

            copy.cooldown = this.cooldown;

            copy.manaCost = this.manaCost;
            copy.availablePoints = this.availablePoints;

            copy.pointCost = this.pointCost;
            copy.IsUnlocked = this.IsUnlocked;
            copy.RequiredSkillTitle = this.RequiredSkillTitle;
            copy.RequiredSkill = this.RequiredSkill;
            copy.onRun = new InstructionList(this.onRun);

            return copy;
        }

        public bool IsReady(GameObject gameObject)
        {
            Args args = new Args(gameObject);
            return Time.time >= lastUsedTime + this.cooldown;
        }

        public void UnlockSkill(float availablePoints)
        {
            if (availablePoints >= pointCost && !IsUnlocked)
            {
                IsUnlocked = true;

                if (Type == SkillType.Passive && !hasBeenExecuted)
                {
                    Execute(GameObject.FindWithTag("Player"), SkillsManager.Instance.skills);
                    hasBeenExecuted = true;
                }
            }
        }

        public bool HasEnoughMana()
        {
            GameObject playerGameObject = GameObject.FindWithTag("Player");
            if (playerGameObject == null) return false;

            var traits = playerGameObject.GetComponent<GameCreator.Runtime.Stats.Traits>();
            if (traits == null) return false;

            double manaValue = traits.RuntimeAttributes.Get(manaAttribute.ID).Value;
            return manaValue >= this.manaCost;
        }

        public void SubtractSkillPoints()
        {
            {
                Args args = new Args(null as GameObject);
                float currentPoints = (float)SkillsManager.Instance.availablePoints.Get(args);
                float newPoints = Mathf.Max(0, currentPoints - this.pointCost);
                this.availablePoints.Set(newPoints, args);
            }
        }

        public void UseMana()
        {
            GameObject playerGameObject = GameObject.FindWithTag("Player");
            if (playerGameObject == null) return;

            var traits = playerGameObject.GetComponent<GameCreator.Runtime.Stats.Traits>();
            if (traits == null) return;

            IdString manaID = manaAttribute.ID;
            double currentMana = traits.RuntimeAttributes.Get(manaID).Value;

            traits.RuntimeAttributes.Get(manaID).Value = Math.Max(0, currentMana - this.manaCost);
        }

        public bool IsRequirementMet(List<Skill> allSkills)
        {
            if (string.IsNullOrEmpty(RequiredSkillTitle))
                return true;

            return allSkills.Any(skill => skill.Title == RequiredSkillTitle);
        }

        public void UnlockSkill(GameObject gameObject)
        {
            Args args = new Args(gameObject);
            if (this.availablePoints.Get(args) >= this.pointCost)
            {
                IsUnlocked = true;
            }
        }

        public bool IsOnCooldown => Time.time < LastUsedTime + CooldownDuration;

        private bool CheckPreExecutionConditions(GameObject gameObject)
        {
            return this.onCheck.Check(new Args(gameObject));
        }

        public void Execute(GameObject gameObject, List<Skill> allSkills)
        {
            // Updated conditional statement in the Execute method
            if (IsUnlocked && !IsOnCooldown && IsRequirementMet(allSkills) && HasEnoughMana() && CheckPreExecutionConditions(gameObject))
            {
                _ = this.onRun.Run(new Args(gameObject));
                LastUsedTime = Time.time;
                UseMana();
            }
            else
            {
                Debug.Log("Skill cannot be executed (locked, on cooldown, requirements not met, insufficient mana, or pre-execution conditions not met).");
            }
        }
    }
}
