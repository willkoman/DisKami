using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using System.Net.Http;
using System.Threading;
using DisKami.Services;
using Interactivity;
using Qmmands;

namespace DisKami
{
        internal class Program
        {
            public static void Main(string[] args)
            {
                var bot = new DisKami();

                Task.Run(async () =>
                {
                    bot.Initialize();
                    await bot.StartAsync();

                }).GetAwaiter().GetResult();
            }

        }

}
