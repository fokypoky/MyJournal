import datetime

import psycopg2
import dbconfig


class Timetable:
    def __init__(self):
        self.timetable = []

    def append_timetable(self, row: []) -> None:
        self.timetable.append(row)


class HomeWork:
    def __init__(self, task: str, task_date: datetime.date, deadline_date: datetime.date):
        self.task = task
        self.task_date = task_date
        self.deadline_date = deadline_date


class HomeWorkRequest:
    def __init__(self):
        self.this_month_home_work = []
        self.deadline_home_work = []


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

    def get_homework_by_class_subject(self, _class: str, subject: str) -> HomeWorkRequest:
        hw_request = HomeWorkRequest()
        # заданые в этом месяце
        with self.connection.cursor() as cursor:
            query = f"""SELECT t.task_text, t.task_start_date, t.task_deadline_date FROM tasks t JOIN subjects s ON t.subject_id = s.id
                            JOIN classes c ON t.class_id = c.id
                        WHERE EXTRACT(YEAR FROM t.task_start_date) = EXTRACT(YEAR FROM NOW())
                        AND EXTRACT(MONTH FROM t.task_start_date) = EXTRACT(MONTH FROM NOW())
                        AND c.class_number = '{_class}' AND s.subject_title = '{subject}'
                            ORDER BY task_start_date"""
            cursor.execute(query)
            for task in cursor.fetchall():
                hw_request.this_month_home_work.append(HomeWork(task[0], task[1], task[2]))
        # задания с дедлайном втечение недели
        with self.connection.cursor() as cursor:
            query = f"""SELECT t.task_text, t.task_start_date, t.task_deadline_date  FROM tasks t JOIN subjects s ON t.subject_id = s.id
                            JOIN classes c ON t.class_id = c.id
                        WHERE EXTRACT(YEAR FROM t.task_deadline_date) = EXTRACT(YEAR FROM NOW())
                        AND EXTRACT(MONTH FROM t.task_deadline_date) = EXTRACT(MONTH FROM NOW())
                        AND (EXTRACT(DAYS FROM t.task_deadline_date) - EXTRACT(DAY FROM NOW())) <= 7
                            AND c.class_number = '{_class}' AND s.subject_title = '{subject}'
                                ORDER BY task_deadline_date"""
            cursor.execute(query)
            for task in cursor.fetchall():
                hw_request.deadline_home_work.append(HomeWork(task[0], task[1], task[2]))
        return hw_request


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

    def get_subjects_by_class(self, _class: str) -> []:
        with self.connection.cursor() as cursor:

            query = f"""SELECT s.subject_title
                        FROM class_subject cs JOIN classes c on cs.class_id = c.id
                            JOIN subjects s on cs.subject_id = s.id
                                WHERE c.class_number = '{_class}'
                                    ORDER BY s.subject_title"""

            cursor.execute(query)
            subjects = []
            for row in cursor.fetchall():
                subjects.append(row[0])
            return subjects


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

    def get_message_from_homework_request(self, request: HomeWorkRequest) -> str:
        message = "Надо сделать в этом месяце:\n"
        for hw in request.this_month_home_work:
            message += f"- {self.convert_homework_to_string(hw)}\n"

        message += "---------------\n"
        message += "Скоро сдавать:\n"

        for hw in request.deadline_home_work:
            message += f"- {self.convert_homework_deadline_to_string(hw)}\n"
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

    def convert_homework_deadline_to_string(self, hw: HomeWork) -> str:
        return f"{hw.task} {self.convert_time_to_string(hw.deadline_date.day)}" \
                         f".{self.convert_time_to_string(hw.deadline_date.month)}" \
                         f".{self.convert_time_to_string(hw.deadline_date.year)}"

    def convert_homework_to_string(self, hw: HomeWork) -> str:
        return f"{hw.task}  {self.convert_time_to_string(hw.task_date.day)}" \
                         f".{self.convert_time_to_string(hw.task_date.month)}" \
                         f".{self.convert_time_to_string(hw.task_date.year)}" \
                         f"-{self.convert_time_to_string(hw.deadline_date.day)}" \
                         f".{self.convert_time_to_string(hw.deadline_date.month)}" \
                         f".{self.convert_time_to_string(hw.deadline_date.year)}"



#sql = SQLDatabase()
# timetable = sql.get_timetable_by_class('11-А')
#request = sql.get_homework_by_class_subject('11-А', 'Математика')
#print(MessageConverter().get_message_from_homework_request(request))
#
# self.task = task
#         self.task_date = task_date
#         self.deadline_date = deadline_date

