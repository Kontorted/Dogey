﻿using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Dogey.Extensions;
using Dogey.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dogey.Modules
{
    [Module, Name("Custom")]
    [RequireContext(ContextType.Guild)]
    public class CustomModule
    {
        private DiscordSocketClient _client;

        public CustomModule(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("create")]
        public async Task Create(IUserMessage msg, string name)
        {
            var guild = (msg.Channel as IGuildChannel)?.Guild;
            string prefix = await guild.GetCustomPrefixAsync();

            if (string.IsNullOrWhiteSpace(name))
            {
                await msg.Channel.SendMessageAsync("Command names cannot be empty.");
                return;
            }

            using (var db = new DataContext())
            {
                int cmds = db.Commands.Where(x => x.GuildId == guild.Id && x.Name == name).Count();

                if (cmds > 0)
                {
                    var cmd = new CustomCommand(msg, name.ToLower());
                    db.Commands.Add(cmd);

                    await db.SaveChangesAsync();
                    await msg.Channel.SendMessageAsync($":thumbsup: You can now add tags with `{prefix}.add <tag> <message>`.");
                } else
                {
                    await msg.Channel.SendMessageAsync($"The command `{name}` already exists.");
                }
            }
        }

        [Command("delete")]
        public async Task Delete(IUserMessage msg, string name)
        {

        }

        [Command("rename")]
        public async Task Rename(IUserMessage msg, string oldname, string newname)
        {

        }

        [Command("commands")]
        public async Task Commands(IUserMessage msg)
        {
            var guild = (msg.Channel as IGuildChannel)?.Guild;
            string prefix = await guild.GetCustomPrefixAsync();
            string message;

            using (var db = new DataContext())
            {
                var cmds = db.Commands.Where(x => x.GuildId == guild.Id).Select(x => x.Name);

                if (cmds.Count() > 0)
                    message = $"```xl\n{string.Join(", ", cmds)}```";
                else
                    message = $"There are no commands for this server, add some with `{prefix}create <command>`.";
            }

            await msg.Channel.SendMessageAsync(message);
        }
        
        [Module("commands"), Name("")]
        public class SubCommands
        {
            private DiscordSocketClient _client;

            public SubCommands(DiscordSocketClient client)
            {
                _client = client;
            }

            [Command("top")]
            public async Task Top(IUserMessage msg, int page = 1)
            {

            }

            [Command("recent")]
            public async Task Recent(IUserMessage msg, int page = 1)
            {

            }

            [Command("mine")]
            public async Task Mine(IUserMessage msg, int page = 1)
            {

            }
        }
    }
}