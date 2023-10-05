import telebot
from telebot import types
import SQLDatabase

bot = telebot.TeleBot('5755828154:AAE1aXWX0bzBiaxIRkQPevTDU2TdAHV_2Iw')
database = SQLDatabase.SQLDatabase()


@bot.message_handler(commands=['start'])
def start_command(message: types.Message) -> None:
    student_first_name = message.from_user.first_name
    student_last_name = message.from_user.last_name

    if student_first_name is None:
        student_first_name = ""
    if student_last_name is None:
        student_last_name = ""

    hello_message = f"<b>{student_first_name} {student_last_name}</b>, привет!"
    hello_message += "\nДобро пожаловать в наш телеграм бот.\nЗдесь Вы можете посмотреть расписание и домашние задания."
    hello_message += "\nДля того чтобы продолжить выберите соответствующую кнопку"

    reply_markup = types.ReplyKeyboardMarkup(resize_keyboard=True)

    timetable_button = types.KeyboardButton('Расписание')
    homework_button = types.KeyboardButton('Домашнее задание')

    reply_markup.add(timetable_button, homework_button)

    bot.send_message(message.chat.id, hello_message, parse_mode='html', reply_markup=reply_markup)


@bot.callback_query_handler(func= lambda call: True)
def callback_handler(call: types.CallbackQuery):
    terminals = call.data.split(' ')

    if terminals[0] == "showtimetable":
        timetable = database.get_timetable_by_class(terminals[1])
        message = SQLDatabase.MessageConverter().get_message_from_timetable(timetable)
        bot.send_message(call.message.chat.id, message)

    if len(terminals) == 2 and terminals[0] == 'showhomework':
        markup = types.InlineKeyboardMarkup()
        subjects = database.get_subjects_by_class(terminals[1])

        if len(subjects) == 0:
            bot.send_message(call.message.chat.id, "Нет предметов для выбора")
            return

        for subject in subjects:
            markup.add(types.InlineKeyboardButton(text=subject, callback_data=f"showhomework {terminals[1]} {subject}"))

        bot.send_message(call.message.chat.id, "Выберите предмет: ", reply_markup=markup)

    if len(terminals) == 3 and terminals[0] == 'showhomework':
        request = database.get_homework_by_class_subject(terminals[1], terminals[2])
        message = SQLDatabase.MessageConverter().get_message_from_homework_request(request)
        bot.send_message(call.message.chat.id, message)

@bot.message_handler(content_types=['text'])
def handle_message(message: types.Message) -> None:
    if message.text.lower() == "расписание":
        markup = types.InlineKeyboardMarkup()

        classes = database.get_all_class_names()
        for _class in classes:
            markup.add(types.InlineKeyboardButton(text=_class,callback_data=f"showtimetable {_class}"))

        bot.send_message(message.chat.id, "Выберите класс:", parse_mode='html', reply_markup=markup)
    if message.text.lower() == "домашнее задание":
        markup = types.InlineKeyboardMarkup()
        classes = database.get_all_class_names()

        for _class in classes:
            markup.add(types.InlineKeyboardButton(text=_class, callback_data=f"showhomework {_class}"))

        bot.send_message(message.chat.id, "Выберите класс", parse_mode='html', reply_markup=markup)

bot.infinity_polling()
