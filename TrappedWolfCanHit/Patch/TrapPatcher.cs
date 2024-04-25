

using Fusion;
using static UnityEngine.UI.GridLayoutGroup;
using UnityEngine;
using static PlayerController;
using Managers;

namespace TrappedWolfCanHit.Patch
{
    internal class TrapPatcher
    {
        public static void Patch()
        {
            On.PlayerController.CheckPlayerRayCast += PlayerController_CheckPlayerRayCast;
        }

        private static bool PlayerController_CheckPlayerRayCast(On.PlayerController.orig_CheckPlayerRayCast orig, PlayerController self, PlayerController targetPlayer, float distance)
        {
            bool flag = distance < 1.5f;
            bool result = false;
            if (flag)
            {
                bool flag2 = targetPlayer.PlayerEffectManager.Invisible;
                bool flag3 = Local.LocalCameraHandler.PovPlayer.PlayerEffectManager.Paranoia;
                GameManager.Instance.gameUI.UpdateUsername(((bool)targetPlayer.IsWolf || flag2 || flag3) ? "???" : targetPlayer.PlayerData.Username.ToString());
                GameManager.Instance.gameUI.ShowUsername(value: true);
            }
            if ((bool)self.IsWolf)
            {
                if (flag && !self.IsAttacking && !targetPlayer.IsDead && targetPlayer.Role != PlayerRole.Wolf)
                {
                    GameManager.Instance.gameUI.UpdateInteraction("UI_KILL", Color.white, InputActionName.None);
                    result = true;
                }
            }
            else if (self.IdVoted == -1 && (bool)GameManager.Instance.CanVote && !targetPlayer.IsDead)
            {
                GameManager.Instance.gameUI.UpdateInteraction("UI_VOTE_FOR", Color.white, InputActionName.SecondaryInteract, targetPlayer.PlayerData.Username.ToString());
                result = true;
            }
            return result;
        }
    }
}
