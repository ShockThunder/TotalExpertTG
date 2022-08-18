
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace TotalExpertTG;

public class Program
{
    public static TelegramBotClient bot = new(TOKEN);
    public const string TOKEN = "5567107382:AAH1lCd-VCFMaGKbW_HqYdyiFQnIVNZH_Nk";
    public const string MEME_CALLBACK = "memeCallback";

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        Console.WriteLine(JsonSerializer.Serialize(update));
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
                    var uri = new Uri("https://pbs.twimg.com/media/Ei8m_X0WkAAnRYg.jpg");
                   
                    await botClient.SendPhotoAsync(callback.Message.Chat, new InputOnlineFile(uri));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // Некоторые действия
        Console.WriteLine(JsonSerializer.Serialize(exception));
    }
    
    static void Main(string[] args)
    {
        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

        // var cts = new CancellationTokenSource();
        // var cancellationToken = cts.Token;
        // var receiverOptions = new ReceiverOptions
        // {
        //     AllowedUpdates = { }, // receive all update types
        // };
        // bot.StartReceiving(
        //     HandleUpdateAsync,
        //     HandleErrorAsync,
        //     receiverOptions,
        //     cancellationToken
        // );

        var textAdder = new TextAdder();
        var result = textAdder.GenerateMeme("text");
        
        Console.ReadLine();
    }
}