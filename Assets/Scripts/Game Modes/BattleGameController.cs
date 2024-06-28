using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameController : BaseGameMode
{
    private BattleGameModeSettings BattleSettings => settings as BattleGameModeSettings;

    protected override void PostSetup(GameModeSettings gameModeSettings)
    {
        foreach (PlayerConfiguration player in PlayerConfigurationManager.Instance.GetPlayerConfigurations())
        {
            characterRanks.Add(new CharRankData(player.PlayerIndex));
        }

        StartCoroutine(EndGameAfterDelay());
    }

    public override Transform GetResetTransform(int playerID)
    {
        return transform;
    }

    public void PlayerPickedUpCoin(int playerID)
    {
        foreach (CharRankData charRankData in characterRanks)
            if (charRankData.PlayerID == playerID)
                charRankData.AddPoints(1);
    }

    private IEnumerator EndGameAfterDelay()
    {
        yield return new WaitForSeconds(BattleSettings.durationMins * 60f);

        OnGameFinished();
    }
}
