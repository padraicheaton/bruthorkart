using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGameController : BaseGameMode
{

    private BattleGameModeSettings BattleSettings => settings as BattleGameModeSettings;

    protected override void PostSetup(GameModeSettings gameModeSettings)
    {

    }

    public override Transform GetResetTransform(int playerID)
    {
        return transform;
    }
}
