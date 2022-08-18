using HtmlAgilityPack;

using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace TotalExpertTG;

public class Program
{
    public static TelegramBotClient bot = new(TOKEN);
    public const string TOKEN = "5567107382:AAH1lCd-VCFMaGKbW_HqYdyiFQnIVNZH_Nk";
    public const string MEME_CALLBACK = "memeCallback";
    private static Random random = new Random();

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if(update.Type == UpdateType.Message)
        {
            var message = update.Message;
            if (message.Text.ToLower() == "/start")
            {
                // var button = new KeyboardButton("Дай мем, эксперт.");
                // var reply = new ReplyKeyboardMarkup(button);
                var button = InlineKeyboardButton.WithCallbackData("Дай мем, эксперт.", MEME_CALLBACK);
                var reply = new InlineKeyboardMarkup(
                    new[]
                    {
                        new[]
                        {
                            button
                        }
                    });
                await botClient.SendTextMessageAsync(message.Chat, "Чего тебе, неуч?", replyMarkup: reply);
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
        }

        if (update.Type == UpdateType.CallbackQuery)
        {
            var callback = update.CallbackQuery;
            if (callback.Data == MEME_CALLBACK)
            {
                try
                {
                    var textAdder = new TextAdder();
                    var text = await GetPanoramaNews();
                    var result = textAdder.GenerateMeme(text);
                    result.Position = 0;
                    await botClient.SendPhotoAsync(callback.Message.Chat, result);
                    var button = InlineKeyboardButton.WithCallbackData("Дай мем, эксперт.", MEME_CALLBACK);
                    var reply = new InlineKeyboardMarkup(
                        new[]
                        {
                            new[]
                            {
                                button
                            }
                        });
                    await botClient.SendTextMessageAsync(callback.Message.Chat, "Чего тебе, неуч?", replyMarkup: reply);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }

    public static async Task<string> GetPanoramaNews()
    {
        var html = @"https://panorama.pub/politics";

        var web = new HtmlWeb();

        var htmlDoc = web.Load(html);
        var nodes = htmlDoc.DocumentNode.SelectNodes("//div[contains(@class, 'pt-2 text-xl lg:text-lg xl:text-base text-center font-semibold')]");

        var node = nodes[random.Next(0, nodes.Count)];
        return node.InnerText.Trim();
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine($"{exception.Message} {exception.InnerException?.Message}");
    }
    
    static void Main(string[] args)
    {
        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );


        
        Console.ReadLine();
    }
}