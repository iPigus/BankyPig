using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnMoveUI : MonoBehaviour
{
    [Header("Basic Info")]
    public string moveName = string.Empty;
    public int turnPoints = 1;
    public bool isReusable = false; 

    [Header("Attacking")]
    public bool isAttack = false;
    public Vector2Int Attack = new();

    [Header("Shielding")]
    public bool isShield = false;
    public Vector2Int Shield = new();

    [Header("Healing")]
    public bool isHeal = false;
    public Vector2Int Heal = new();

    [Header("Serielized Fields")]
    [SerializeField] TextMeshProUGUI nameText;
    [SerializeField] TextMeshProUGUI reusableText;
    [SerializeField] TextMeshProUGUI turnPointsText;

    [SerializeField] GameObject AttackStat;
    [SerializeField] GameObject HealStat;
    [SerializeField] GameObject ShieldStat;

    HorizontalLayoutGroup layoutGroup;
    RectTransform rectTransform;

    private void Awake()
    {
        layoutGroup = transform.parent.GetComponent<HorizontalLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();

        InfoUpdate();
    }

    public void Click()
    {
        if (!TurnBasedManager.canMakeMove(turnPoints)) return;
        if (!isAttack && !isShield && !isHeal) { Debug.LogError("It ain't doing nothing"); return; }
        System.Random random = new();

        if (isAttack)
        {
            int amount = random.Next(Attack.x, Attack.y + 1);

            if (amount > 0) TurnBasedManager.PlayerAttack(amount);
        }
        if (isShield) 
        {
            int amount = random.Next(Attack.x, Attack.y + 1);
            
            if (amount != 0) TurnBasedManager.PlayerShield(amount);
        }
        if (isHeal) 
        {
            int amount = random.Next(Attack.x, Attack.y + 1);

            if (amount != 0) TurnBasedManager.PlayerHeal(amount);
        }

        TurnBasedManager.UseTurnPoints(turnPoints);

        if (isReusable) ReShowAnimation();
        else Hide();
    }

    public void Hide()
    {
        rectTransform.anchoredPosition = new(rectTransform.anchoredPosition.x, -420);
    }
    public void Disable()
    {

    }
    public void Enable()
    {
        InfoUpdate();


    }

    public async void ReShowAnimation()
    {
        if (layoutGroup.enabled) layoutGroup.enabled = false;

        Vector2 startPos = new(rectTransform.anchoredPosition.x, -420); Vector2 endPos = new(rectTransform.anchoredPosition.x, -120);
        const float animationTime = .35f; float timeElapsed = 0f;

        while (true)
        {
            float dialation = Mathf.Pow(timeElapsed / animationTime, .8f);
            rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, dialation);
            
            await Task.Yield();
            timeElapsed += Time.deltaTime;
            if (timeElapsed > animationTime) break;
        }

        rectTransform.anchoredPosition = endPos;
    }



    void InfoUpdate()
    {
        nameText.text = moveName;
        reusableText.gameObject.SetActive(isReusable);
        turnPointsText.gameObject.SetActive(turnPoints != 1);
        turnPointsText.text = turnPoints.ToString();

        AttackStat.gameObject.SetActive(isAttack); if (isAttack) AttackStat.GetComponentInChildren<TextMeshProUGUI>().text = Attack.x + " - " + Attack.y;
        ShieldStat.gameObject.SetActive(isShield); if (isShield) AttackStat.GetComponentInChildren<TextMeshProUGUI>().text = Attack.x + " - " + Attack.y;
        HealStat.gameObject.SetActive(isHeal); if (isHeal) AttackStat.GetComponentInChildren<TextMeshProUGUI>().text = Attack.x + " - " + Attack.y;

        if (isHeal && isShield && isAttack)
        {
            Debug.LogError("To implement!");
        }
    }
}
