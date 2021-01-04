using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using DisKami.Services;
using Interactivity;
using Microsoft.Extensions.DependencyInjection;
using Qmmands;

namespace DisKami
{
    class DisKami
    {
        public IServiceProvider Provider { get; private set; }
        public DiscordSocketClient Client { get; private set; }
        public CommandService Commands { get; private set; }

        public void Initialize()
        {
            IServiceCollection services = new ServiceCollection();
            Provider = ConfigureServices(services);

            Commands = Provider.GetRequiredService<CommandService>();
            Commands.AddModules(Assembly.GetEntryAssembly()); //Add Modules to the CommandService

            Client = Provider.GetRequiredService<DiscordSocketClient>();
        }

        public async Task StartAsync()
        {
            string path = @"config.dkk";
            string[] readText = File.ReadAllLines(path);
            string key = readText[0];
            await Client.LoginAsync(TokenType.Bot, key); 
            await Client.StartAsync(); 
            await Client.SetGameAsync("%help"); 
            Client.Log += x =>
            {
                Console.WriteLine(x.Message);
                return Task.CompletedTask;
            };
            Commands.CommandErrored += (result, ctx, provider) =>
            {

                Console.WriteLine(result.Exception.ToString());

                return Task.CompletedTask;
            };

            Client.MessageReceived += async s =>
            {
                if (!(s is SocketUserMessage msg))
                {
                    
                    return; //Do some checks
                }
                if (!msg.Author.IsBot)
                {
                    Console.WriteLine(msg.Author + " said: " + msg.Content);
                    try
                    {
                        SocketGuild server = ((SocketGuildChannel)msg.Channel).Guild;
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter($"logs/{server.Name} - Log.txt", true))
                        {
                            await file.WriteLineAsync(System.DateTime.Now + " | " + msg.Channel + " | " + msg.Author + " said: " + msg.Content);

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                if (msg.Author.IsBot)
                {
                    return;
                }

                if (msg.Author.Id == Client.CurrentUser.Id)
                {
                    return;
                }

                var context = new CommandHandlingService(msg);

                if (!CommandUtilities.HasAnyPrefix(msg.Content, new[] { "%" }, StringComparison.OrdinalIgnoreCase, out string usedPrefix, out string cmd) == true)
                {
                    return;
                }

                var result = await Commands.ExecuteAsync(cmd, context, Provider); //Try to run Command

                if (result is FailedResult failResult)
                {
                    await context.Channel.SendMessageAsync(failResult.Reason);
                }

                return;
            };

            await Task.Delay(-1);                                     //Wait forever to keep the bot running
        }

        private IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Client = new DiscordSocketClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose, MessageCacheSize = 50 });

            return services
                .AddSingleton(Client)
                .AddSingleton(new CommandService(new CommandServiceConfiguration { CaseSensitive = false, IgnoreExtraArguments = false, DefaultRunMode = RunMode.Parallel }))
                .AddSingleton(new InteractivityService(Client, TimeSpan.FromSeconds(20)))
                .AddSingleton<HttpClient>()
                .AddSingleton<MusicService>()
                .AddSingleton<PictureService>()
                .AddSingleton<NHentaiService>()
                .AddSingleton<YoutubeService>()
                .BuildServiceProvider();
        }
    }
}
