using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using OptionsBot;

public class TelegramHelpBot
{
    public TelegramBotClient bot;
    private InlineKeyboardMarkup _murkup1 = new InlineKeyboardMarkup()
    .AddButton("Пожар🔥", "Fire")
    .AddButton("ДТП🚙", "Accident")
    .AddNewRow()
    .AddButton("Стало Плохо🫀", "BecomeBad")
    .AddButton("Сведения о Боте🤖", "info");
                                                                                                    

    public TelegramHelpBot(TelegramBotConfig cfg) 
    {
        bot = new TelegramBotClient(cfg.BotToken);
    }

    public async Task MessageHandler(Message message, UpdateType type) 
    {
        if (message.Text == "/start") 
        {
            await bot.SendTextMessageAsync(message.Chat.Id, BotOptionsHandler.QuestHandler(OptionsBot.Questions.HAPPEN), replyMarkup: _murkup1);
        }
    }

    public async Task UpdateHandler(Update update) 
    {
        switch (update.Type)
        {
            case UpdateType.CallbackQuery:

                string data = update.CallbackQuery.Data;
                long chatId = update.CallbackQuery.From.Id;
                string callBackId = update.CallbackQuery.Id;

                if (data == "Fire")
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.Fire), inline: new InlineKeyboardMarkup().AddButton("Пожар в Доме🏠", "fireInHouse").AddButton("Пожар в Школе🏫", "fireInSchool"));

                else if (data == "fireInHouse")
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.FireInHouse));

                else if (data == "fireInSchool")
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.FireInSchool));

                else if (data == "Home")
                {
                    await bot.AnswerCallbackQueryAsync(callBackId);
                    await bot.SendTextMessageAsync(chatId, BotOptionsHandler.QuestHandler(OptionsBot.Questions.HAPPEN), replyMarkup: _murkup1);
                }
                else if (data == "Accident")
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.Accident));

                else if (data == "BecomeBad")
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.BecameBad), new InlineKeyboardMarkup().AddButton("Как оказать первую помощь?🆘", "FirstAid"));
                
                else if (data == "FirstAid") 
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.FirstAid));
                
                else if(data == "info")
                    await MessageSend(callBackId, chatId, BotOptionsHandler.ButtonHandler(Buttons.InformationBot));

                break;
        }
    }

    public async Task MessageSend(string callBackId, long ChatId, string text, InlineKeyboardMarkup? inline = null) 
    {
        await bot.AnswerCallbackQueryAsync(callBackId);
        if(inline is not null)
            await bot.SendTextMessageAsync(ChatId, text, replyMarkup: inline.AddNewRow().AddButton("На главную⭕️", "Home"));
        else
            await bot.SendTextMessageAsync(ChatId, text, replyMarkup: new InlineKeyboardMarkup().AddButton("На главную⭕️", "Home"));
    }


    public void BotActivating() 
    {
        bot.OnMessage += MessageHandler;
        bot.OnUpdate += UpdateHandler;
        Console.ReadLine();
    }


}
