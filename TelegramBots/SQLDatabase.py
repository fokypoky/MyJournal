import datetime

import psycopg2
import dbconfig


class Timetable:
    def __init__(self):
        self.timetable = []

    def append_timetable(self, row: []) -> None:
        self.timetable.append(row)


class SQLDatabase:
    def __init__(self) -> None:
        try:
            self.connection = psycopg2.connect(
                host=dbconfig.host,
                user=dbconfig.user,
                password=dbconfig.password,
                database=dbconfig.db_name)
            self.connection.autocommit = True
        except Exception as _ex:
            print(_ex)

    def get_all_class_names(self) -> []:
        with self.connection.cursor() as cursor:
            query = """SELECT class_number FROM classes ORDER BY class_number"""
            cursor.execute(query)

            classes = []
            for row in cursor.fetchall():
                classes.append(row[0])

            return classes

    def get_timetable_by_class(self, _class: str) -> Timetable:
        with self.connection.cursor() as cursor:
            query = f"""SELECT
                            t.day_of_week,
                            s.subject_title, t.lesson_time,
                            a.auditory_number, cont.surname
                        FROM timetable t join classes c ON c.id = t.class_id
                            JOIN subjects s ON s.id = t.subject_id
                            JOIN auditories a ON a.id = t.auditory_id
                            JOIN employees e ON e.id = t.teacher_id
                            JOIN contacts cont ON cont.id = e.contacts_id
                                WHERE c.class_number = '{_class}'
                                ORDER BY t.day_of_week, t.lesson_time"""
            cursor.execute(query)
            timetable = Timetable()
            for row in cursor.fetchall():
                timetable.append_timetable(list(row))
            return timetable


class MessageConverter:
    def get_message_from_timetable(self, timetable: Timetable) -> str:
        message = "Ваше расписание:\n"

        for i in range(1, 8):
            rows = [row for row in timetable.timetable if row[0] == i]
            if len(rows) == 0:
                message += "---------------\n"
                message += f"{self.get_day_of_week_by_int(i)}:\nЗанятий нет\n"
            else:
                message += "---------------\n"
                message += f"{self.get_day_of_week_by_int(i)}:\n"
                for row in rows:
                    message += f"{row[1]} {self.convert_time_to_string(row[2].hour)}:{self.convert_time_to_string(row[2].minute)} {row[3]}ауд. {row[4]}\n"
        return message

    def get_day_of_week_by_int(self, day: int) -> str:
        if day == 1:
            return "Понедельник"
        elif day == 2:
            return "Вторник"
        elif day == 3:
            return "Среда"
        elif day == 4:
            return "Четверг"
        elif day == 5:
            return "Пятница"
        elif day == 6:
            return "Суббота"
        elif day == 7:
            return "Воскресенье"
        return "error"

    def convert_time_to_string(self, time: int) -> str:
        return str(time) if time > 9 else f"0{time}"


sql = SQLDatabase()
timetable = sql.get_timetable_by_class('11-А')

# время
# print(f"{timetable.timetable[0][2].hour}:{timetable.timetable[0][2].minute}")

