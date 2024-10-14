using UnityEngine;
using UnityEngine.UI;

namespace ETK
{
    public class SkillPopupUI : MonoBehaviour
    {
        public Text titleText;
        public Text subtitleText;
        public Text descriptionText;
        public Image descriptionImage;
        public Text pointCostText;
        public GameObject unlockText;
        public GameObject unlockImage;

        public void SetupPopup(Skill skill)
        {
            titleText.text = skill.Title;
            subtitleText.text = skill.Subtitle;
            descriptionText.text = skill.DescriptionText;
            descriptionImage.sprite = skill.DescriptionImage;

            if (pointCostText != null) pointCostText.text = skill.pointCost.ToString();

            bool isSkillUnlocked = skill.IsUnlocked;
            if (unlockText != null) unlockText.SetActive(!isSkillUnlocked);
            if (unlockImage != null) unlockImage.SetActive(!isSkillUnlocked);
        }

        public void RefreshPopup(Skill skill)
        {
            SetupPopup(skill);
        }
    }
}
