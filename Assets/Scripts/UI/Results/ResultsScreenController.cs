using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultsScreenController : Singleton<ResultsScreenController>
{
    [Header("References")]
    [SerializeField] private CanvasGroup parentCanvasGroup;
    [SerializeField] private Transform dataRowContainer;
    [SerializeField] private GameObject dataRowPrefab;

    [Header("Settings")]
    [SerializeField] private float displayDuration;

    private void Start()
    {
        parentCanvasGroup.alpha = 0f;

        Setup(BaseGameMode.Instance.GetRankedCharacters());
    }

    public void Setup(List<CharRankData> ranks)
    {
        StartCoroutine(FadeIn());

        foreach (CharRankData rank in ranks)
        {
            GameObject row = Instantiate(dataRowPrefab, dataRowContainer);
            string name = rank.PlayerID >= 0 ? $"Player {rank.PlayerID + 1}" : $"NPC {Mathf.Abs(rank.PlayerID)}";

            row.GetComponent<ResultsDataRow>().Setup(name, rank.Points);
        }

        PlayerConfigurationManager.Instance.RemoveAIPlayerConfigs();

        StartCoroutine(ReturnToMainAfterDelay());
    }

    private IEnumerator FadeIn()
    {
        while (parentCanvasGroup.alpha < 1f)
        {
            parentCanvasGroup.alpha += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator ReturnToMainAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);

        SceneController.Instance.TransitionScene(SceneController.Level.ModeSelection);
    }
}
