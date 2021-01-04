using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DisKami.Services;
using Interactivity;
using Interactivity.Confirmation;
using Interactivity.Pagination;
using Interactivity.Selection;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using Qmmands;
using CommandAttribute = Qmmands.CommandAttribute;
using RemainderAttribute = Qmmands.RemainderAttribute;
using NHentaiSharp.Search;

namespace DisKami.Modules
{
    public class InfoModule : Qmmands.ModuleBase<CommandHandlingService>
    {
        // Dependency Injection will fill this value in for us
        public PictureService PictureService { get; set; }
        public InteractivityService Interactivity { get; set; }
        public Qmmands.CommandService Service { get; set; }
        public NHentaiService NHentaiService { get; set; }
        

        public string[] quotes =
        {
            "“Anyone who has ever made anything of importance was disciplined.” — Andrew Hendrixson",
            "“Creativity is intelligence having fun.” — Albert Einstein",
            "“What you get by achieving your goals is not as important as what you become by achieving your goals.” — Henry David Thoreau",
            "“I destroy my enemies when I make them my friends.” — Abraham Lincoln",
            "“Don’t live the same year 75 times and call it a life.” — Robin Sharma",
            "“You will succeed because most people are lazy.” — Shahir Zag",
            "“A comfort zone is a beautiful place, but nothing ever grows there.” — Author Unknown",
            "“You must be the change you wish to see in the world.” — Mahatma Gandhi",
            "“If you want to live a happy life, tie it to a goal, not to people or objects.” — Albert Einstein",
            "“Sometimes you win, sometimes you learn.” — John Maxwell"
        };


        [Command("ping")]
        [Description("Pings the bot for a Pong")]
        public Task PingAsync()
            => Context.Channel.SendMessageAsync("pong!");

        [Command("cat")]
        [Description("Send a random cat picture")]
        public async Task CatAsync()
        {
            // Get a stream containing an image of a cat
            var stream = await PictureService.GetCatPictureAsync();
            // Streams must be seeked to their beginning before being uploaded!
            stream.Seek(0, SeekOrigin.Begin);
            await Context.Channel.SendFileAsync(stream, "cat.png");
        }

        [Command("userinfo")]
        [Description("Gives info on a user")]
        public async Task UserInfoAsync(string mention=null)
        {
            IUser user;
            if (mention == null)
                user = Context.User;
            else
                user = Context.Guild.GetUser(MentionUtils.ParseUser(mention));

            var b = new Discord.EmbedBuilder();
            b.WithTitle("User Info");
            b.WithDescription("Name: "+user.Username+"\n"+
                "ID: " + user.ToString() + "\n" +
                "UID: " + user.Id + "\n" +
                "Status: " + user.Status + "\n" +
                "Current Activity: " + user.Activity + "\n" );
            await Context.Channel.SendMessageAsync("", false, b.Build());
        }

        [Command("av")]
        [Description("Zooms in on a user's Avatar")]
        public async Task UserAvatarAsync(string mention=null)
        {
            IUser user;
            if (mention == null)
                user = Context.User;
            else
                user = Context.Guild.GetUser(MentionUtils.ParseUser(mention));

            var b = new Discord.EmbedBuilder();
            b.WithTitle(user.Username + "'s Avatar");
            b.WithImageUrl(user.GetAvatarUrl());
            await Context.Channel.SendMessageAsync("", false, b.Build());
            //await Context.Channel.SendMessageAsync(user.Username);
        }

        [Command("quote")]
        [Description("Replies with a growing list of quotes")]
        public async Task Quote()
        {
            Random r = new Random();
            string quote = quotes[r.Next(quotes.Length - 1)];
            await Context.Channel.SendMessageAsync(quote);
        }
        [Command("hentai")]
        [Description("Searches NHentai for doujins with tags separated by a space")]
        public async Task Hentai([Remainder] string tags)
        {
            GalleryElement qq = await NHentaiService.getBook(tags);
          
            Console.WriteLine(qq.numPages);
            var paginator = new LazyPaginatorBuilder()
                .WithUsers(Context.User)
                .WithPageFactory(PageFactory)
                .WithMaxPageIndex(int.Parse(qq.numPages.ToString()))
                .WithFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
                .WithDefaultEmotes()
                .Build();
            Console.WriteLine(qq.cover.imageUrl);
            //await Context.Channel.SendMessageAsync(stuff.value[0].thumbnailUrl.ToString());
            await Interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));

            Task<PageBuilder> PageFactory(int page)
            {
                return Task.FromResult(new PageBuilder()
                    //.WithText((page + 1).ToString())
                    .WithTitle(qq.prettyTitle+$" Page {page + 1}")
                    .WithImageUrl(qq.pages[page].imageUrl.ToString())
                    .WithColor(System.Drawing.Color.FromArgb(page * 400)));
            }
        }

        [Command("im")]
        [Description("searches for images with a specified query")]
        public async Task ImageSearch([Remainder] string query)
        {
            string qq = await PictureService.GetGoogleResult(query);
            dynamic stuff = JsonConvert.DeserializeObject(qq);
            Console.WriteLine(stuff.totalEstimatedMatches);
            var paginator = new LazyPaginatorBuilder()
                .WithUsers(Context.User)
                .WithPageFactory(PageFactory)
                .WithMaxPageIndex(int.Parse(stuff.totalEstimatedMatches.ToString())-1)
                .WithFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
                .WithDefaultEmotes()
                .Build();
            Console.WriteLine(stuff.value[0].thumbnailUrl);
            //await Context.Channel.SendMessageAsync(stuff.value[0].thumbnailUrl.ToString());
            await Interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));
           
            Task<PageBuilder> PageFactory(int page)
            {
                return Task.FromResult(new PageBuilder()
                    //.WithText((page + 1).ToString())
                    .WithTitle($"Image Result {page + 1}")
                    .WithImageUrl(stuff.value[page].thumbnailUrl.ToString())
                    .WithColor(System.Drawing.Color.FromArgb(page * 1500)));
                
            }
        }
        //[Command("page")]
        //public Task LazyPaginatorAsync()
        //{
        //    var paginator = new LazyPaginatorBuilder()
        //        .WithUsers(Context.User)
        //        .WithPageFactory(PageFactory)
        //        .WithMaxPageIndex(100)
        //        .WithFooter(PaginatorFooter.PageNumber | PaginatorFooter.Users)
        //        .WithDefaultEmotes()
        //        .Build();

        //    return Interactivity.SendPaginatorAsync(paginator, Context.Channel, TimeSpan.FromMinutes(2));

        //    Task<PageBuilder> PageFactory(int page)
        //    {
        //        return Task.FromResult(new PageBuilder()
        //            .WithText((page + 1).ToString())
        //            .WithTitle($"Title for page {page + 1}")
        //            .WithColor(System.Drawing.Color.FromArgb(page * 1500)));
        //    }
        //}

        // Ban a user
        [Command("ban")]
        [Description("Bans specified user.")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
        [RequireUserPermission(GuildPermission.BanMembers)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task BanUserAsync(string mention = null, [Remainder] string reason = null)
        {
               IGuildUser user;

               user = Context.Guild.GetUser(MentionUtils.ParseUser(mention));
            var b = new Discord.EmbedBuilder();
            b.WithTitle("User Banned");
            b.WithDescription(user.Username + "was banned.");
            await Context.Channel.SendMessageAsync("", false, b.Build());
            await user.Guild.AddBanAsync(user, reason: reason);
            
        }
        // Kick a user
        [Command("kick")]
        [Description("Kicks specified user")]
        [RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
        [RequireUserPermission(GuildPermission.KickMembers)]
        [RequireBotPermission(GuildPermission.KickMembers)]
        public async Task KickUserAsync(string mention = null, [Remainder] string reason = null)
        {
            IGuildUser user;

            user = Context.Guild.GetUser(MentionUtils.ParseUser(mention));
            var b = new Discord.EmbedBuilder();
            b.WithTitle("User Kicked");
            b.WithDescription(user.Username + "was kicked.");
            await Context.Channel.SendMessageAsync("", false, b.Build());
            await user.KickAsync();
        }

        [Command("purge")]
        [Description("Clears x amount of messages. Default 20")]
        [RequireUserPermission(GuildPermission.ManageMessages)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        public async Task PurgeMessagesAsync(int amount = 20)
        {
            var messages = await this.Context.Channel.GetMessagesAsync((int)amount + 1).FlattenAsync();

            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
            const int delay = 5000;
            var m = await Context.Channel.SendMessageAsync($"Purge completed. _This message will be deleted in {delay / 1000} seconds._");
            await Task.Delay(delay);
            await m.DeleteAsync();
        }
        // [Remainder] takes the rest of the command's arguments as one argument, rather than splitting every space
        //[Command("echo")]
        //public Task EchoAsync([Remainder] string text)
        //    // Insert a ZWSP before the text to prevent triggering other bots!
        //    => ReplyAsync('\u200B' + text);

        //[Command("list")]
        //public Task ListAsync(params string[] objects)
        //    => ReplyAsync("You listed: " + string.Join("; ", objects));
        [Command("say")]
        [Description("Repeats what's said after 'say'")]
        public async Task Say([Remainder] string query)
        {
            var messages = await this.Context.Channel.GetMessagesAsync(1).FlattenAsync();

            await (Context.Channel as ITextChannel).DeleteMessagesAsync(messages);
            await Context.Channel.SendMessageAsync(query);
        }



        [Command("help")]
        [Description("Lists available commands.")]
        public async Task help()
        {
            await Context.Channel.SendMessageAsync("Check your DM for a list of commands!");
            var a = new EmbedBuilder();
            a.WithTitle("Commands");
            a.WithDescription(string.Join('\n', Service.GetAllCommands().Select(x => $"`{x.Name}` - {x.Description}")));
            Discord.IDMChannel gencom = await Context.Message.Author.GetOrCreateDMChannelAsync();
            await gencom.SendMessageAsync("", false, a.Build());
            await gencom.CloseAsync();
        }
    }
}
