using Microsoft.Xna.Framework.Graphics;
using System;
using System.Reflection;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace DeathGhost;

public class DeathGhost : Mod
{
    public override void Load()
    {
        On_Main.DrawInterface_35_YouDied += (DrawInterface_35_YouDied) =>
        {
            Player player = Main.LocalPlayer;
            int baseImmuneAlpha = player.immuneAlpha;
            if (player.ghost)
                player.immuneAlpha = 255;
            DrawInterface_35_YouDied();
            player.immuneAlpha = baseImmuneAlpha;
            return;
        };
    }
}

public class GhostPlayer : ModPlayer
{
	readonly MethodInfo Player_ResetProjectileCaches = typeof(Player).GetMethod("ResetProjectileCaches", BindingFlags.NonPublic | BindingFlags.Instance);
	readonly MethodInfo Player_UpdateProjectileCaches = typeof(Player).GetMethod("UpdateProjectileCaches", BindingFlags.NonPublic | BindingFlags.Instance);

    public override void PreUpdate()
	{
		if (Player.dead)
		{
			if (Player.difficulty == 2)
            {
				return;
            }
			if (Player.ghost)
			{
				Player.UpdateDead();
				Player_ResetProjectileCaches.Invoke(Player, null);
				Player_UpdateProjectileCaches.Invoke(Player, [ Player.whoAmI ]);
			}
			else
            {
				Player.ghost = true;
            }
		}
	}

    public override void OnRespawn()
    {
        if (Player.difficulty == 2)
        {
            return;
        }
        Player.ghost = false;
    }
}
