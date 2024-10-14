using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ETK
{
    public class SkillButtonUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler,
                                  IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public string skillTitle;
        public Skill skill;
        public Image icon;
        public Image bgPassive;
        public Image cdPassive;
        public Image bgActive;
        public Image cdActive;
        public GameObject popupPrefab;
        private GameObject popupInstance;
        private Canvas canvas;
        private RectTransform rectTransform;
        private CanvasGroup canvasGroup;
        private Vector2 originalPosition;

        private void Start()
        {
            if (SkillsManager.Instance != null)
            {
                skill = SkillsManager.Instance.skills.Find(s => s.Title == skillTitle);
                if (skill != null)
                {
                    UpdateUI();
                }
            }
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            canvas = FindObjectOfType<Canvas>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (skill != null && popupPrefab != null)
            {
                popupInstance = Instantiate(popupPrefab, transform.position, Quaternion.identity, transform.parent);
                RectTransform popupRect = popupInstance.GetComponent<RectTransform>();
                RectTransform buttonRect = GetComponent<RectTransform>();

                float offsetDistance = 350f;
                Vector3 offsetRight = new Vector3(offsetDistance, 0, 0);
                Vector3 offsetLeft = new Vector3(-offsetDistance, 0, 0);
                Vector3 popupPosition = transform.position + offsetRight;

                if (popupPosition.x + popupRect.rect.width > Screen.width)
                {
                    popupPosition = transform.position + offsetLeft; 
                }

                popupInstance.transform.position = popupPosition;

                SkillPopupUI popupUI = popupInstance.GetComponent<SkillPopupUI>();
                if (popupUI != null)
                {
                    popupUI.SetupPopup(skill);
                }
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (popupInstance != null)
            {
                Destroy(popupInstance);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (skill != null)
            {
                if (!skill.IsUnlocked)
                {
                    bool hasRequiredSkill = string.IsNullOrEmpty(skill.RequiredSkillTitle) ||
                                            SkillsManager.Instance.skills.Any(s => s.Title == skill.RequiredSkillTitle && s.IsUnlocked);

                    Args args = new Args(gameObject);
                    float availablePointsValue = (float)SkillsManager.Instance.availablePoints.Get(args);

                    if (hasRequiredSkill && availablePointsValue >= skill.pointCost)
                    {
                        skill.UnlockSkill(availablePointsValue);
                        skill.SubtractSkillPoints();
                        RefreshUI();

                        if (popupInstance != null)
                        {
                            SkillPopupUI popupUI = popupInstance.GetComponent<SkillPopupUI>();
                            if (popupUI != null)
                            {
                                popupUI.RefreshPopup(skill);
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot unlock skill: Requirements not met.");
                    }
                }
                else
                {
                    Debug.Log("Skill is already unlocked.");
                }
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (skill != null && skill.Type == Skill.SkillType.Active)
            {
                originalPosition = rectTransform.anchoredPosition;
                canvasGroup.alpha = 0.6f;
                canvasGroup.blocksRaycasts = false;
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (skill != null && skill.Type == Skill.SkillType.Active)
            {
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (skill != null && skill.Type == Skill.SkillType.Active)
            {
                canvasGroup.alpha = 1f;
                canvasGroup.blocksRaycasts = true;
                rectTransform.anchoredPosition = originalPosition;
            }
        }

        public void UpdateUI()
        {
            if (skill == null) return;

            icon.sprite = skill.IconImage;

            Color lockedColor = new Color32(0xF5, 0xF5, 0xF5, 0xFF);
            Color unlockedColor = new Color32(0xFA, 0xCB, 0x45, 0xFF); 

            Color targetColor = skill.IsUnlocked ? unlockedColor : lockedColor;
            icon.color = targetColor;
            bgPassive.color = targetColor;
            cdPassive.color = targetColor;
            bgActive.color = targetColor;
            cdActive.color = targetColor;

            bool isActiveSkill = skill.Type == Skill.SkillType.Active;
            bgPassive.gameObject.SetActive(!isActiveSkill);
            cdPassive.gameObject.SetActive(!isActiveSkill);
            bgActive.gameObject.SetActive(isActiveSkill);
            cdActive.gameObject.SetActive(isActiveSkill);
        }

        private void OnEnable()
        {
            SkillsManager.OnSkillReset += RefreshUI;
        }

        private void OnDisable()
        {
            SkillsManager.OnSkillReset -= RefreshUI;
        }

        public void RefreshUI()
        {
            if (SkillsManager.Instance != null)
            {
                skill = SkillsManager.Instance.skills.Find(s => s.Title == skillTitle);
                UpdateUI();
            }
        }
    }
}
