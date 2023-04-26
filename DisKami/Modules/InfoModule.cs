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
        public string[] responses =
        {
            "Yo this guy smells really bad",
            "Do you hear something?",
            "*shut up man*",
            "*yikes*",
            "Dude why can't we just ban this kid",
            "Holy shit just fucking die jfc you're so retarded like why are you even here?",
            "Are you always so stupid or is today a special occasion?",
            "<:downvote:522279950515175424>",
            "<:downvote:522279950515175424>"
        };
        public string[] hydecaption =
       {
            "I'm that gorilla dick nigga, I make dike pussy wet.",
            "If you like sports you're a fucking faggot",
            "When we win, do not forget that these people want you broke, dead, your kids raped and brainwashed, and they think it's funny",
            "Stand up like men! and reclaim our soil.\nKinsmen arise! Look toward the stars and proclaim our destiny.\nIn Metaline Falls we have a saying, “Defeat never. Victory forever!",
            "My mom says im part jewish",
            "opioids are the religion of the masses",
            "I slept with a tranny. That's right kids! I sucked another man's dick, and convinced myself that it doesn't make me gay!",
            "I like to shill my inane, gay bullshit on /pol/.",
            "everyone who gave me money is a fucking idiot. You fucking retards actually thought I wouldn't waste it on cars and bitcoin. I know Charles went back to working a manual labor job but fuck him, I'm way funnier and smarter.",
            "My mom is cool and my mom will treat you right",
            "Imagine you just got accepted into an Ivy League school and you rejected them. That's Purple gangsta. Imagine you got the dopest piece of pussy you ever had and she's all into that. And she's 420 friendly. That's Shemale Kush."
        };
        public string[] funfacts =
        {
            "McDonald’s once made bubblegum-flavored broccoli",
            "Some fungi create zombies, then control their minds",
            "The first oranges weren’t orange",
            "There’s only one letter that doesn’t appear in any U.S. state name",
            "A cow-bison hybrid is called a “beefalo”",
            "Johnny Appleseed’s fruits weren’t for eating",
            "Scotland has 421 words for “snow”",
            "Samsung tests phone durability with a butt-shaped robot",
            "The “Windy City” name has nothing to do with Chicago weather",
            "Peanuts aren’t technically nuts",
            "Armadillo shells are bulletproof",
            "Firefighters use wetting agents to make water wetter",
            "The longest English word is 189,819 letters long",
            "“Running amok” is a medically recognized mental condition",
            "Octopuses lay 56,000 eggs at a time",
            "Cats have fewer toes on their back paws",
            "Kleenex tissues were originally intended for gas masks",
            "Blue whales eat half a million calories in one mouthful",
            "That tiny pocket in jeans was designed to store pocket watches",
            "Turkeys can blush",
            "Iceland’s last McDonald’s burger was sold eight years ago …",
            "The man with the world’s deepest voice can make sounds humans can’t hear",
            "The American flag was designed by a high school student",
            "Thanks to 3D printing, NASA can basically “email” tools to astronauts"
        };
        public string[] funfactsd =
        {
            "This interesting fact will have your taste buds crawling. Unsurprisingly, the attempt to get kids to eat healthier didn’t go over well with the child testers, who were 'confused by the taste.'",
            "The tropical fungus Ophiocordyceps infects ants’ central nervous systems. By the time the fungi been in the insect bodies for nine days, they have complete control over the host’s movements. They force the ants to climb trees, then convulse and fall into the cool, moist soil below, where fungi thrive. Once there, the fungus waits until exactly solar noon to force the ant to bite a leaf and kill it",
            "The original oranges from Southeast Asia were a tangerine-pomelo hybrid, and they were actually green. In fact, oranges in warmer regions like Vietnam and Thailand still stay green through maturity.",
            "Can you guess the answer to this random fact? You’ll find a Z (Arizona), a J (New Jersey), and even two X’s (New Mexico and Texas)—but not a single Q.",
            "You can even buy its meat in at least 21 states",
            "Yes, there was a real John Chapman who planted thousands of apple trees on U.S. soil. But the apples on those trees were much more bitter than the ones you’d find in the supermarket today. “Johnny Appleseed” didn’t expect his fruits to be eaten whole, but rather made into hard apple cider.",
            "421?! Some examples: *sneesl* (to start raining or snowing); *feefle* (to swirl); *flinkdrinkin* (a light snow).",
            "Do these interesting facts have you rethinking everything? People stash their phones in their back pockets all the time, which is why Samsung created a robot that is shaped like a butt—and yes, even wears jeans—to “sit” on their phones to make sure they can take the pressure.",
            "Was this one of the random facts you already knew? Chicago’s nickname was coined by 19th-century journalists who were referring to the fact that its residents were “windbags” and “full of hot air.”",
            "They’re legumes. According to Merriam-Webster, a nut is only a nut if it’s “a hard-shelled dry fruit or seed with a separable rind or shell and interior kernel.” That means walnuts, almonds, cashews, and pistachios aren’t nuts either. They’re seeds.",
            "In fact, one Texas man was hospitalized when a bullet he shot at an armadillo ricocheted off the animal and hit him in the jaw.",
            "The chemicals reduce the surface tension of plain water so it’s easier to spread and soak into objects, which is why it’s known as “wet water.”",
            "We won’t spell it out here (though you can read it here), but the full name for the protein nicknamed titin would take three and a half hours to say out loud",
            "Considered a culturally bound syndrome, a person “running amok” in Malaysia commits a sudden, frenzied mass attack, then begins to brood.",
            "The mother spends six months so devoted to protecting the eggs that she doesn’t eat. The babies are the size of a grain of rice when they’re born.",
            "Like most four-legged mammals, they have five toes on the front, but their back paws only have four toes. Scientists think the four-toe back paws might help them run faster",
            "When there was a cotton shortage during World War I, Kimberly-Clark developed a thin, flat cotton substitute that the army tried to use as a filter in gas masks. The war ended before scientists perfected the material for gas masks, so the company redeveloped it to be smoother and softer, then marketed Kleenex as facial tissue instead.",
            "Those 457,000 calories are more than 240 times the energy the whale uses to scoop those krill into its mouth.",
            "The original jeans only had four pockets: that tiny one, plus two more on the front and just one in the back.",
            "When turkeys are scared or excited—like when the males see a female they’re interested in—the pale skin on their head and neck turns bright red, blue, or white. The flap of skin over their beaks, called a “snood,” also reddens.",
            "… and you can still see it today. Its home is in a hostel, but you can catch a glimpse on the 24/7 live webcam stream dedicated to it. These interesting facts keep getting weirder and weirder.",
            "The man, Tim Storms, can’t even hear the note, which is eight octaves below the lowest G on a piano—but elephants can.",
            "It started as a school project for Bob Heft’s junior-year history class, and it only earned a B- in 1958. His design had 50 stars even though Alaska and Hawaii weren’t states yet. Heft figured the two would earn statehood soon and showed the government his design. After President Dwight D. Eisenhower called to say his design was approved, Heft’s teacher changed his grade to an A.",
            "Getting new equipment to the Space Station used to take months or years, but the new technology means the tools are ready within hours."
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
        [Command("hydeme")]
        [Description("Sends a picture of Sam Hyde with a degenerate quote below it")]
        public async Task HydeMeAsync()
        {   
            Random r = new Random();
            await Context.Channel.SendFileAsync("hyde\\hyde (" + r.Next(1, 9) + ").jpg", hydecaption[r.Next(hydecaption.Length)]);
        }
        [Command("funfact")]
        [Description("Sends a fun fact from a random list")]
        public async Task FunFactAsync()
        {   
            Random r = new Random();
            int hh = r.Next(funfacts.Length);
            await Context.Channel.SendMessageAsync(funfacts[hh]+"```"+funfactsd[hh]+"```");
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
