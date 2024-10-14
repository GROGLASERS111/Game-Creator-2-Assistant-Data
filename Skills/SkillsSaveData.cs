using System;
using System.Collections.Generic;

namespace ETK
{
    [Serializable]
    public class SkillsSaveData
    {
        public float availablePoints;
        public List<SavedSkillData> savedSkills;

        [Serializable]
        public class SavedSkillData
        {
            public string title;
            public bool isUnlocked;
            public float lastUsedTime;
        }
    }
}
