using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Qmmands;
using Discord.Rest;
using DiscordRPC;

namespace DisKami.Services
{
    public class CommandHandlingService : Qmmands.ICommandContext
    {
        public ISocketMessageChannel Channel { get; }
        public SocketUser User { get; }
        public SocketGuild Guild { get; }
        public IMessage Message { get; }

        public CommandHandlingService(ISocketMessageChannel channel, SocketUser user, SocketGuild guild)
        {
            Channel = channel;
            User = user;
            Guild = guild;
            Message = null;
        }
        public CommandHandlingService(SocketMessage message)
        {
            Channel = message.Channel;
            User = message.Author;
            if (Channel is IPrivateChannel)
                Guild = null;
            else
            Guild = (User as SocketGuildUser).Guild;
            Message = message;
        }

    }
}
