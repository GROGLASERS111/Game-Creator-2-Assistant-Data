using GameCreator.Runtime.Common;
using UnityEngine;
using System.Collections.Generic;

namespace ETK
{
    public class SkillsManager : MonoBehaviour
    {
        public static SkillsManager Instance { get; private set; }

        public PropertyGetDecimal availablePoints;
        public PropertySetNumber availablePointsSet;
        public delegate void SkillResetHandler();
        public static event SkillResetHandler OnSkillReset;
        public List<Skill> skills;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void AddSkill(Skill skillTemplate)
        {
            Skill newSkill = skillTemplate.DeepCopy();
            skills.Add(newSkill);
        }

        public void SetAvailablePoints(float newPoints)
        {
            this.availablePoints = new PropertyGetDecimal(newPoints);
        }

        public void ResetAndRefundSkills()
        {
            float refundedPoints = 0;
            foreach (var skill in skills)
            {
                if (skill.IsUnlocked)
                {
                    refundedPoints += skill.pointCost;
                    skill.IsUnlocked = false;
                    skill.ResetExecution();
                }
            }

            Args args = new Args(null as GameObject);
            float currentPoints = (float)this.availablePoints.Get(args);
            float newPoints = currentPoints + refundedPoints;
            this.availablePointsSet.Set(newPoints, args);

            OnSkillReset?.Invoke();
        }

        public SkillsSaveData GetSkillsSaveData()
        {
            SkillsSaveData saveData = new SkillsSaveData();

            foreach (Skill skill in skills)
            {
                // Populate saveData with information from each skill
            }

            return saveData;
        }

        public void ApplySkillsSaveData(SkillsSaveData saveData)
        {
            this.availablePointsSet.Set(saveData.availablePoints, null as Args);

            foreach (var savedSkillData in saveData.savedSkills)
            {
                Skill skill = skills.Find(s => s.Title == savedSkillData.title);
                if (skill != null)
                {
                    skill.IsUnlocked = savedSkillData.isUnlocked;
                    skill.UpdateLastUsedTime(savedSkillData.lastUsedTime);
                }
            }
        }

        public void ExecuteSkill(string title, GameObject gameObject)
        {
            Skill skill = skills.Find(s => s.Title == title);
            if (skill != null)
            {
                skill.Execute(gameObject, skills);
            }
        }
    }
}
