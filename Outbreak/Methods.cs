﻿using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MEC;

namespace Outbreak
{
    public class Methods
    {
        private readonly Plugin plugin;
        public Methods(Plugin plugin) => this.plugin = plugin;

        public void SetupPlayers()
        {
            Timing.CallDelayed(2f, () => SpawnAlphas());
        }

        void SpawnAlphas()
        {
            int counter = 0;
            foreach (Player player in Player.List)
                if (player.Team == Team.SCP)
                {
                    counter++;
                    if (counter >= plugin.Config.MaxAlphaCount)
                    {
                        player.Role = RoleType.ClassD;
                        continue;
                    }

                    player.SetRole(RoleType.Scp0492, true);
                    Timing.CallDelayed(0.5f, () => player.Health = plugin.Config.AlphaZombieHealth);
                }
        }
        
        internal void EventRegistration(bool disable = false)
        {
            switch (disable)
            {
                case true:
                    Exiled.Events.Handlers.Player.InteractingDoor -= plugin.EventHandlers.OnDoorInteraction;
                    Exiled.Events.Handlers.Server.RoundStarted -= plugin.EventHandlers.OnRoundStart;
                    Exiled.Events.Handlers.Server.RoundEnded -= plugin.EventHandlers.OnRoundEnd;
                    Exiled.Events.Handlers.Player.Hurting -= plugin.EventHandlers.OnPlayerHurt;
                    Exiled.Events.Handlers.Player.Joined -= plugin.EventHandlers.OnPlayerJoin;
                    Exiled.Events.Handlers.Player.Died -= plugin.EventHandlers.OnPlayerDeath;
                    break;
                case false:
                    Exiled.Events.Handlers.Player.InteractingDoor += plugin.EventHandlers.OnDoorInteraction;
                    Exiled.Events.Handlers.Server.RoundStarted += plugin.EventHandlers.OnRoundStart;
                    Exiled.Events.Handlers.Server.RoundEnded += plugin.EventHandlers.OnRoundEnd;
                    Exiled.Events.Handlers.Player.Hurting += plugin.EventHandlers.OnPlayerHurt;
                    Exiled.Events.Handlers.Player.Joined += plugin.EventHandlers.OnPlayerJoin;
                    Exiled.Events.Handlers.Player.Died += plugin.EventHandlers.OnPlayerDeath;
                    break;
            }
        }

        public void EnableGamemode(bool force = false)
        {
            if (!force)
                plugin.IsEnabled = true;
            else
            {
                plugin.IsEnabled = true;
                SetupPlayers();
            }

            plugin.ShouldDisableNextRound = true;
        }

        public void DisableGamemode(bool force = false)
        {
            if (!force)
                plugin.ShouldDisableNextRound = true;
            else
            {
                List<RoleType> scpRoles = new List<RoleType>
                {
                    RoleType.Scp049,
                    RoleType.Scp079,
                    RoleType.Scp096,
                    RoleType.Scp106,
                    RoleType.Scp173,
                    RoleType.Scp93953,
                    RoleType.Scp93989
                };
                
                foreach (Player player in Player.List)
                {
                    if (player.Role == RoleType.Scp173)
                    {
                        player.Role = scpRoles[plugin.Rng.Next(scpRoles.Count)];
                    }
                    else
                    {
                        int r = plugin.Rng.Next(6);
                        switch (r)
                        {
                            case 6:
                                player.Role = RoleType.FacilityGuard;
                                break;
                            case 5:
                            case 4:    
                                player.Role = RoleType.Scientist;
                                break;
                            default:
                                player.Role = RoleType.ClassD;
                                break;
                        }
                    }
                }
            }
        }
    }
}