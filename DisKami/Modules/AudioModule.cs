using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DisKami.Services;
using System;
using System.Collections.Generic;
using System.IO;
using Qmmands;
using System.Text;
using System.Threading.Tasks;
using CommandAttribute = Qmmands.CommandAttribute;
using RemainderAttribute = Qmmands.RemainderAttribute;

namespace DisKami.Modules
{
    public class AudioModule : Qmmands.ModuleBase<CommandHandlingService>
    {
        public YoutubeService YoutubeService { get; set; }
        
        
        
        [Command("yt")]
        [Description("searches for youtube video with specified query")]
        public async Task YoutubeAsync([Remainder] string query)
        {
            string url = await YoutubeService.GetVideo(query);
            await Context.Channel.SendMessageAsync(url);
        }
    }
}
