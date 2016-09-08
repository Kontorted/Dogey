﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogey.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogey.Modules.InfoModule
{
    [Module, Name("Info")]
    [RequireContext(ContextType.Guild)]
    public class UserGroup
    {
        private DiscordSocketClient _client;

        public UserGroup(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("userinfo")]
        [Description("Get information about this user.")]
        public async Task Userinfo(IUserMessage msg, IUser user = null)
        {
            var guild = (msg.Channel as IGuildChannel)?.Guild;
            var u = user as IGuildUser ?? msg.Author as IGuildUser;

            var infomsg = new List<string>
            {
                "```xl",
                $"   Name: {u} ({u.Id})",
                $"   Nick: {u.Nickname}",
                $"Created: {u.CreatedAt}",
                $" Joined: {u.JoinedAt}",
                $"Playing: {u.Game.Name}",
                $" Avatar: {u.AvatarUrl}",
                $"  Roles: {u.Roles.Count()}",
                $"  Roles: {Enum.GetName(typeof(UserStatus), u.Status)}",
                "```"
            };

            await Utility.SendMessage(msg, string.Join("\n", infomsg));
        }

        [Module("userinfo"), Name("Info")]
        public class SubCommands
        {
            [Command("permissions")]
            [Description("Get this user's permissions.")]
            public async Task Permissions(IUserMessage msg, IUser user = null)
            {
                var u = user as IGuildUser ?? msg.Author as IGuildUser;
                var g = u.GuildPermissions;

                var infomsg = new List<string>
                {
                    "```xl",
                    $"  Administrator: {Convert.ToInt32(g.Administrator)}",
                    "```"
                };

                await Utility.SendMessage(msg, string.Join("\n", infomsg));
            }
        }
    }
}