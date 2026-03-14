using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using static System.Net.Mime.MediaTypeNames;

namespace CS_GameTime
{
    internal class TgBot
    {
        long MY_userId = 1816585045;

        RemoteControl remoteControl = new RemoteControl();

        public void StartBot()
        {
            Console.WriteLine("Бот запущен...");
            var botClient = new TelegramBotClient("7901504622:AAHv3f8CgqNZyutZ9yQhFvRTIvenkqn9-JU");
            using var cts = new CancellationTokenSource(); 

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = Array.Empty<UpdateType>(),
                ThrowPendingUpdates = true, //Игнорирует старые сообщения при запуске
            };


            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,    // Метод для обработки сообщений
                pollingErrorHandler: HandleErrorAsync, // Метод для ошибок
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );
        }

        async void SendMess(ITelegramBotClient bot, CancellationToken ct, string text)
        {

            try
            {
                await bot.SendTextMessageAsync(
                chatId: MY_userId,
                text: text,
                parseMode: ParseMode.Markdown,
                cancellationToken: ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка отправки сообщения: {ex.Message}");
            }

        }
        string EscapeMarkdown(string text)
        {
            return text
                .Replace("_", "\\_")
                .Replace("*", "\\*")
                .Replace("[", "\\[")
                .Replace("]", "\\]")
                .Replace("(", "\\(")
                .Replace(")", "\\)")
                .Replace("`", "\\`");
        }
        async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken ct)
        {

            if (update.Message is not { From: { } user, Text: { } messageText, Chat: { } chat })
                return;


            string message = update.Message.Text.ToLower();

            if (update.Message.Chat.Type == ChatType.Private)
            {
                if(user.Id == MY_userId)
                {
                    string text = "";
                    switch(message)
                    {
                        case "/help":
                            text = $"Доступные команды:\n\n" +
                            "/start - начать управление компьютером\n" +
                            "/info - информация о вас\n" +
                            "/start_celltosingularity - запустить CellToSingularity\n" +
                            "/stop_celltosingularity - остановить CellToSingularity\n" +
                            "/start_cs2 - запустить CS2\n" +
                            "/stop_cs2 - остановить CS2\n" +
                            "/status - статус процессов игр";
                            SendMess(bot, ct, EscapeMarkdown(text));
                            break;
                        case "/start":
                             text = $"{user.FirstName}, можно управлять компьютером! /help - доступные команды";
                             SendMess(bot, ct, text);
                            break;
                        case "/info":
                            text = $"💠 Информация о вас:\n\n" +
                            $"👤 Имя: {user.FirstName}\n" +
                            $"🆔 ID: {user.Id}\n" +
                            $"🔖 Юзернейм: @{user.Username}\n";
                            SendMess(bot, ct, text);
                            break;

                        case "/start_celltosingularity":
                            if(remoteControl.IsGameRunning("celltosingularity"))
                            {
                                text = $"{user.FirstName}, целка уже запущена!";
                                SendMess(bot, ct, text);
                                break;
                            }
                            else
                            {
                                remoteControl.StartSteamGame(977400);
                                text = $"{user.FirstName}, игра celltosingularity запущена!";
                                SendMess(bot, ct, text);
                            }
                            break;
                        case "/stop_celltosingularity":
                            remoteControl.StopGame("celltosingularity");
                            text = $"{user.FirstName}, игра celltosingularity закрыта!";
                            SendMess(bot, ct, text);
                            break;

                        case "/start_cs2":
                            if(remoteControl.IsGameRunning("cs2"))
                            {
                                text = $"{user.FirstName}, игра CS2 уже запущена!";
                                SendMess(bot, ct, text);
                                break;
                            }
                            else
                            {
                                remoteControl.StartSteamGame(730);
                                text = $"{user.FirstName}, игра CS2 запущена!";
                                SendMess(bot, ct, text);
                            }
                            break;
                        case "/stop_cs2":
                            remoteControl.StopGame("cs2");

                            text = $"{user.FirstName}, игра CS2 закрыта!";
                            SendMess(bot, ct, text);
                            break;

                        //all started processes
                        case "/status":
                            text = "*Статус процессов:*\n\n" +
                                $"{remoteControl.GetStatGame("celltosingularity")}\n" +
                                $"{remoteControl.GetStatGame("cs2")}";
                            
                            SendMess(bot, ct, text);
                            break;
                    }
                }
            }

        }

        Task HandleErrorAsync(ITelegramBotClient bot, Exception ex, CancellationToken ct)
        {
            Console.WriteLine($"Ошибка бота: {ex.Message}");
            StartBot(); // Перезапуск бота при ошибке
            return Task.CompletedTask;
        }
    }
}
