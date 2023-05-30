import telebot
from telebot import types

bot = telebot.TeleBot('5755828154:AAE1aXWX0bzBiaxIRkQPevTDU2TdAHV_2Iw')

@bot.message_handler(commands=['start'])
def startBot(message: types.Message) -> None:
    student_first_name = message.from_user.first_name
    student_last_name = message.from_user.last_name

    if student_first_name is None:
        student_first_name = ""
    if student_last_name is None:
        student_last_name = ""

    hello_message = f"<b>{student_first_name} {student_last_name}</b>, привет!"
    hello_message += "\nДобро пожаловать в наш телеграм бот.\nЗдесь Вы можете посмотреть расписание, домашние задания и свои оценки."
    hello_message += "\nДля того чтобы продолжить выберите соответствующую кнопку"
    bot.send_message(message.chat.id, hello_message, parse_mode='html')

bot.infinity_polling()