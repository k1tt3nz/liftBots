using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;

//Кирен
var botClient = new TelegramBotClient("5376308156:AAHp6jDxtHa1YV3nux_Cnz_hoiuwzVkEMI4");

using var cts = new CancellationTokenSource();


var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();


cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;

    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    using (StreamReader reader = new StreamReader("E:\\VS repos\\LiftBots\\SharedProject\\character dossiers\\Kiren.txt"))
    {
        Message sentMessage = await botClient.SendTextMessageAsync(
        chatId: chatId,
        text: await reader.ReadToEndAsync(),
        cancellationToken: cancellationToken);
    }

    Message messageGEO = await botClient.SendLocationAsync(
        chatId: chatId,
        latitude: 50.582291,
        longitude: 36.594966,
        cancellationToken: cancellationToken);
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
{
    var ErrorMessage = exception switch
    {
        ApiRequestException apiRequestException
            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        _ => exception.ToString()
    };

    Console.WriteLine(ErrorMessage);
    return Task.CompletedTask;
}
